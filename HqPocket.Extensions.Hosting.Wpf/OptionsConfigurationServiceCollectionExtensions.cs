using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using System;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsConfigurationServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWithDefaultSection<TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);

        return services.Configure<TOptions>(config.GetSection(typeof(TOptions).Name));
    }

    public static IServiceCollection ConfigureWithDefaultSection<TOptions>(this IServiceCollection services, HostBuilderContext context) where TOptions : class
    {
        ArgumentNullException.ThrowIfNull(services);

        return services.ConfigureWithDefaultSection<TOptions>(context.Configuration);
    }
}