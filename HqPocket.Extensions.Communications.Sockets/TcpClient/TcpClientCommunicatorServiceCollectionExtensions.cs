using HqPocket.Extensions.Communications;
using HqPocket.Extensions.Communications.Sockets;
using Microsoft.Extensions.DependencyInjection.Extensions;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class TcpClientCommunicatorServiceCollectionExtensions
{
    public static IServiceCollection AddTcpClientCommunicator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator, TcpClientCommunicator>());

        return services;
    }

    public static IServiceCollection AddTcpClientCommunicator<TProtocol>(this IServiceCollection services)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol>, TcpClientCommunicator<TProtocol>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol>>());

        return services;
    }

    public static IServiceCollection AddTcpClientCommunicator<TProtocol, TData>(this IServiceCollection services)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol, TData>, TcpClientCommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator<TProtocol>>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());

        return services;
    }

    public static IServiceCollection AddTcpClientCommunicator(this IServiceCollection services, Action<TcpClientCommunicatorOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpClientCommunicator();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddTcpClientCommunicator<TProtocol>(this IServiceCollection services, Action<TcpClientCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpClientCommunicator<TProtocol>();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddTcpClientCommunicator<TProtocol, TData>(this IServiceCollection services, Action<TcpClientCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpClientCommunicator<TProtocol, TData>();
        services.Configure(setupAction);

        return services;
    }
}

