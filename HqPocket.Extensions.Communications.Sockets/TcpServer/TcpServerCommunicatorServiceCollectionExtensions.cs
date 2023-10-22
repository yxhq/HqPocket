using HqPocket.Extensions.Communications;
using HqPocket.Extensions.Communications.Sockets;
using Microsoft.Extensions.DependencyInjection.Extensions;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class TcpServerCommunicatorServiceCollectionExtensions
{
    public static IServiceCollection AddTcpServerCommunicator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator, TcpServerCommunicator>());

        return services;
    }

    public static IServiceCollection AddTcpServerCommunicator<TProtocol>(this IServiceCollection services)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol>, TcpServerCommunicator<TProtocol>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol>>());

        return services;
    }

    public static IServiceCollection AddTcpServerCommunicator<TProtocol, TData>(this IServiceCollection services)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ICommunicator<TProtocol, TData>, TcpServerCommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator<TProtocol>>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());
        services.AddSingleton<ICommunicator>(sp => sp.GetRequiredService<ICommunicator<TProtocol, TData>>());

        return services;
    }

    public static IServiceCollection AddTcpServerCommunicator(this IServiceCollection services, Action<TcpServerCommunicatorOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpServerCommunicator();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddTcpServerCommunicator<TProtocol>(this IServiceCollection services, Action<TcpServerCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpServerCommunicator<TProtocol>();
        services.Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddTcpServerCommunicator<TProtocol, TData>(this IServiceCollection services, Action<TcpServerCommunicatorOptions> setupAction)
        where TProtocol : IProtocol
        where TData : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddTcpServerCommunicator<TProtocol, TData>();
        services.Configure(setupAction);

        return services;
    }
}

