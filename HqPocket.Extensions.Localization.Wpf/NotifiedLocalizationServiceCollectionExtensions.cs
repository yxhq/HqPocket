using HqPocket.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class NotifiedLocalizationServiceCollectionExtensions
{
    public static IServiceCollection AddNotifiedLocalization(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddOptions();

        AddNotifiedLocalizationServices(services);

        return services;
    }

    public static IServiceCollection AddNotifiedLocalization(this IServiceCollection services, Action<LocalizationOptions> setupAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (setupAction == null)
        {
            throw new ArgumentNullException(nameof(setupAction));
        }

        AddNotifiedLocalizationServices(services, setupAction);

        return services;
    }

    // To enable unit testing
    internal static void AddNotifiedLocalizationServices(IServiceCollection services)
    {
        services.TryAddSingleton<INotifiedStringLocalizerFactory, NotifiedResourceManagerStringLocalizerFactory>();
        services.TryAddSingleton<IStringLocalizerFactory>(sp => sp.GetRequiredService<INotifiedStringLocalizerFactory>());
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
    }

    internal static void AddNotifiedLocalizationServices(IServiceCollection services, Action<LocalizationOptions> setupAction)
    {
        AddNotifiedLocalizationServices(services);
        services.Configure(setupAction);
    }
}