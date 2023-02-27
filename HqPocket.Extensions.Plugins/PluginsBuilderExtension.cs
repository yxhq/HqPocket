using System.IO;
using System.Reflection;

namespace HqPocket.Extensions.Plugins;

public static class PluginsBuilderExtension
{
    public static IPluginsBuilder AddPlugin(this IPluginsBuilder builder, Assembly assembly)
    {
        builder.AddPlugin(assembly.Location);
        return builder;
    }

    public static IPluginsBuilder AddPlugins(this IPluginsBuilder builder, string[] paths)
    {
        foreach (var path in paths)
        {
            builder.AddPlugin(path);
        }
        return builder;
    }

    public static IPluginsBuilder AddPluginsDirectory(this IPluginsBuilder builder, string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        else
        {
            string[] paths = Directory.GetFiles(directory, "*.dll");
            builder.AddPlugins(paths);
        }
        return builder;
    }
}