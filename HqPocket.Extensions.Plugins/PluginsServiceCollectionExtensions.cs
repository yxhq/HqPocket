using HqPocket.Extensions.Plugins;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class PluginsServiceCollectionExtensions
{
    public static IServiceCollection AddPlugins(this IServiceCollection services, HostBuilderContext context, Action<IPluginsBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(services);

        IPluginsBuilder builder = new PluginsBuilder(context, services);
        configure(builder);
        services.TryAddSingleton(builder.Build());
        return services;
    }
}