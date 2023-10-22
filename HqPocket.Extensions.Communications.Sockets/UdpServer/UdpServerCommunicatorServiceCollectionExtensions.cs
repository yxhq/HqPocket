using HqPocket.Extensions.Communications;
using HqPocket.Extensions.Communications.Sockets;
using Microsoft.Extensions.DependencyInjection.Extensions;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class UdpServerCommunicatorServiceCollectionExtensions
{
    public static IServiceCollection AddUdpServerCommunicator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator, UdpServerCommunicator>());

        return services;
    }

    public static IServiceCollection AddUdpServerCommunicator<TProtocol>(this IServiceCollection services)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol>, UdpServerCommunicator<TProtocol>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol>>());

        return services;
    }

    public static IServiceCollection AddUdpServerCommunicator<TProtocol, TData>(this IServiceCollection services)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol, TData>, UdpServerCommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator<TProtocol>>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());

        return services;
    }

    public static IServiceCollection AddUdpServerCommunicator(this IServiceCollection services, Action<UdpServerCommunicatorOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpServerCommunicator();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddUdpServerCommunicator<TProtocol>(this IServiceCollection services, Action<UdpServerCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpServerCommunicator<TProtocol>();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddUdpServerCommunicator<TProtocol, TData>(this IServiceCollection services, Action<UdpServerCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpServerCommunicator<TProtocol, TData>();
        services.Configure(setupAction);

        return services;
    }
}

