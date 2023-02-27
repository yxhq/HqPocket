using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace HqPocket.Extensions.Plugins;

public interface IPlugin
{
    void ConfigureServices(HostBuilderContext context, IServiceCollection services);
    void Initialize(IServiceProvider serviceProvider);
}