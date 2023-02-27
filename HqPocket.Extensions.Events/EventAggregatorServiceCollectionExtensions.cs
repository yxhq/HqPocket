using HqPocket.Extensions.Events;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class EventAggregatorServiceCollectionExtensions
{
    public static IServiceCollection AddEventAggregator(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IEventAggregator, EventAggregator>();
        return services;
    }
}