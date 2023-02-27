using Microsoft.Extensions.DependencyInjection;

using System;

namespace HqPocket.Extensions.Plugins;

public class PluginsInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var plugins = serviceProvider.GetService<IPlugins>();
        plugins?.Initialize(serviceProvider);
    }
}