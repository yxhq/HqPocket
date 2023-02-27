
using HqPocket.Extensions.Ports;

using System;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ProtocolSerialPortServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncSerialPort(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAsyncSerialPort, AsyncSerialPort>();
        return services;
    }

    public static IServiceCollection AddProtocolSerialPort(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(typeof(IProtocolSerialPort<>), typeof(ProtocolSerialPort<>));
        return services;
    }

    public static IServiceCollection AddProtocolDataSerialPort(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(typeof(IProtocolDataSerialPort<,>), typeof(ProtocolDataSerialPort<,>));
        return services;
    }
}

