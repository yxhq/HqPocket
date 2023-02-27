using System;
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.Extensions.Plugins;

public class Plugins : IPlugins
{
    public IList<string> Paths { get; } = new List<string>();

    public IList<PluginContext> Contexts { get; } = new List<PluginContext>();

    public void Initialize(IServiceProvider serviceProvider)
    {
        foreach (var context in Contexts.Where(p => p.Initialized == false))
        {
            context.Plugin!.Initialize(serviceProvider);
            context.Initialized = true;
        }
    }
}