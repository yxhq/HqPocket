using HqPocket.Extensions.Regioning;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class RegioningServiceCollectionExtensions
{
    public static IServiceCollection AddRegioning(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IRegionManager, RegionManagerImpl>();
        services.TryAddSingleton<IRegionAdapterFactory, RegionAdapterFactory>();
        return services;
    }
}