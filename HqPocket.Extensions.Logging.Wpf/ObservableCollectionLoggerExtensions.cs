using HqPocket.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class ObservableCollectionLoggerExtensions
    {
        public static ILoggingBuilder AddObservableCollectionLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ObservableCollectionLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ObservableCollectionLoggerConfiguration, ObservableCollectionLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddObservableCollectionLogger(this ILoggingBuilder builder, Action<ObservableCollectionLoggerConfiguration> configure)
        {
            builder.AddObservableCollectionLogger();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
