using System;
using System.IO;
using System.Reflection;

namespace HqPocket.Extensions.Plugins;

public class PluginContext : CollectibleAssemblyLoadContext
{
    public IPlugin? Plugin { get; set; }
    public Assembly Assembly { get; }
    public Version? Version { get; }
    public string? AssemblyName { get; }
    public bool Initialized { get; set; }

    public PluginContext(string path) : base(path)
    {
        Assembly = LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(AssemblyPath)));
        Version = Assembly.GetName().Version;
        AssemblyName = Assembly.GetName().Name;
    }
}