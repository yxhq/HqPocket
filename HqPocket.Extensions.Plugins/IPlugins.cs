using System;
using System.Collections.Generic;

namespace HqPocket.Extensions.Plugins;

public interface IPlugins
{
    IList<string> Paths { get; }
    IList<PluginContext> Contexts { get; }
    void Initialize(IServiceProvider serviceProvider);
}