using HqPocket.Extensions.Options;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class WritableOptionsServiceCollectionExtensions
{
    public static IServiceCollection AddWritableOptions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions();
        services.TryAddSingleton(typeof(IWritableOptions<>), typeof(WritableOptionsManager<>));
        return services;
    }
}
