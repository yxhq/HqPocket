
using HqPocket.Communications;
using HqPocket.Extensions.Sockets;
using Microsoft.Extensions.DependencyInjection.Extensions;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class UdpClientCommunicatorServiceCollectionExtensions
{
    public static IServiceCollection AddUdpClientCommunicator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator, UdpClientCommunicator>());
        services.AddSingleton(sp => sp.GetRequiredService<ICommunicator>());

        return services;
    }

    public static IServiceCollection AddUdpClientCommunicator<TProtocol>(this IServiceCollection services)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol>, UdpClientCommunicator<TProtocol>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol>>());

        return services;
    }

    public static IServiceCollection AddUdpClientCommunicator<TProtocol, TData>(this IServiceCollection services)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol, TData>, UdpClientCommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator<TProtocol>>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());

        return services;
    }

    public static IServiceCollection AddUdpClientCommunicator(this IServiceCollection services, Action<SocketCommunicatorOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpClientCommunicator();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddUdpClientCommunicator<TProtocol>(this IServiceCollection services, Action<SocketCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpClientCommunicator<TProtocol>();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddUdpClientCommunicator<TProtocol, TData>(this IServiceCollection services, Action<SocketCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddUdpClientCommunicator<TProtocol, TData>();
        services.Configure(setupAction);

        return services;
    }
}

