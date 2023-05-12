using HqPocket.Extensions.Options;
using HqPocket.Extensions.Options.Json;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Text.Json;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class JsonWritableOptionsServiceCollectionExtensions
{
    public static IServiceCollection AddJsonOptionsWriter(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<JsonSerializerOptions>();
        services.TryAddSingleton<IOptionsWriter, JsonOptionsWriter>();

        return services;
    }

    public static IServiceCollection AddJsonOptionsWriter(this IServiceCollection services, Action<JsonSerializerOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        services.AddJsonOptionsWriter();
        services.Configure(setupAction);

        return services;
    }
}
