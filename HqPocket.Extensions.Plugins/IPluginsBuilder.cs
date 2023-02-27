using Microsoft.Extensions.DependencyInjection;

namespace HqPocket.Extensions.Plugins;

public interface IPluginsBuilder
{
    IServiceCollection Services { get; }
    IPluginsBuilder AddPlugin(string path);
    IPlugins Build();
}