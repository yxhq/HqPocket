using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace HqPocket.Extensions.Logging
{
    [ProviderAlias("ObservableCollection")]
    public class ObservableCollectionLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? _onChangeToken;
        private ObservableCollectionLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, ObservableCollectionLogger> _loggers = new(StringComparer.Ordinal);
        public ObservableCollectionLoggerProvider(IOptionsMonitor<ObservableCollectionLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updateConfig => _currentConfig = updateConfig);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new(name, _currentConfig));
        }

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
    }
}
