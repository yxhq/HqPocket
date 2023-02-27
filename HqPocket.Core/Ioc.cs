using System;
using System.Collections.Generic;
using System.Threading;

namespace HqPocket;

public class Ioc
{
    private static IServiceProvider? _serviceProvider;
    public static bool IsServiceProviderSet { get; private set; }

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        IServiceProvider? existServiceProvider = Interlocked.CompareExchange(ref _serviceProvider, serviceProvider, null);
        if (existServiceProvider is not null)
        {
            throw new InvalidOperationException("ServiceProvider has been set.");
        }
        IsServiceProviderSet = true;
    }

    public static IEnumerable<object?> GetServices(Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        Type genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
        return (IEnumerable<object>)GetRequiredService(genericEnumerable);
    }

    public static IEnumerable<T> GetServices<T>()
    {
        return GetRequiredService<IEnumerable<T>>();
    }

    public static object? GetService(Type serviceType)
    {
        if (!IsServiceProviderSet)
        {
            throw new InvalidOperationException("ServiceProvider has not been set.");
        }
        return _serviceProvider!.GetService(serviceType);
    }

    public static object GetRequiredService(Type serviceType)
    {
        var service = GetService(serviceType);
        if (service is null)
        {
            throw new InvalidOperationException($"The Required service type {serviceType.Name} was not registered.");
        }
        return service;
    }

    public static T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    public static T GetRequiredService<T>()
    {
        T? service = GetService<T>();
        if (service is null)
        {
            throw new InvalidOperationException($"The Required service type {typeof(T).Name} was not registered.");
        }
        return service;
    }
}
