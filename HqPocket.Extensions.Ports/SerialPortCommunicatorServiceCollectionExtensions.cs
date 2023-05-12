
using HqPocket.Communications;
using HqPocket.Extensions.Ports;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class SerialPortCommunicatorServiceCollectionExtensions
{
    public static IServiceCollection AddSerialPortCommunicator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator, SerialPortCommunicator>());
        services.AddSingleton(sp => sp.GetRequiredService<ICommunicator>());

        return services;
    }

    public static IServiceCollection AddSerialPortCommunicator<TProtocol>(this IServiceCollection services)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol>, SerialPortCommunicator<TProtocol>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol>>());

        return services;
    }

    public static IServiceCollection AddSerialPortCommunicator<TProtocol, TData>(this IServiceCollection services)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol, TData>, SerialPortCommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator<TProtocol>>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());

        return services;
    }

    public static IServiceCollection AddSerialPortCommunicator(this IServiceCollection services, Action<SerialPortCommunicatorOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddSerialPortCommunicator();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddSerialPortCommunicator<TProtocol>(this IServiceCollection services, Action<SerialPortCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddSerialPortCommunicator<TProtocol>();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddSerialPortCommunicator<TProtocol, TData>(this IServiceCollection services, Action<SerialPortCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddSerialPortCommunicator<TProtocol, TData>();
        services.Configure(setupAction);

        return services;
    }
}

