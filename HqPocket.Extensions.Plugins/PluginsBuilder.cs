using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;

namespace HqPocket.Extensions.Plugins;

public class PluginsBuilder : IPluginsBuilder
{
    private readonly IPlugins _plugins = new Plugins();
    private readonly HostBuilderContext _hostBuilderContext;
    public IServiceCollection Services { get; }

    public PluginsBuilder(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        _hostBuilderContext = hostBuilderContext;
        Services = services;
    }

    public IPluginsBuilder AddPlugin(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!_plugins.Paths.Contains(path))
        {
            _plugins.Paths.Add(path);
        }
        return this;
    }

    public IPlugins Build()
    {
        foreach (var path in _plugins.Paths)
        {
            PluginContext context = new(path);
            Type? type = context.Assembly.GetTypes().SingleOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
            if (type is not null)
            {
                IPlugin? plugin = (IPlugin?)Activator.CreateInstance(type);
                if (plugin is not null)
                {
                    plugin.ConfigureServices(_hostBuilderContext, Services);
                    context.Plugin = plugin;
                    _plugins.Contexts.Add(context);
                }
            }
        }
        return _plugins;
    }
}