using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace HqPocket.Extensions.Logging
{
    public class ObservableCollectionLogger : ILogger
    {
        internal static readonly ObservableCollection<LogItem> LogItems = new();
        private readonly object _locker = new();
        private readonly string _name;
        private readonly ObservableCollectionLoggerConfiguration _configuration;

        public ObservableCollectionLogger(string name, ObservableCollectionLoggerConfiguration configuration)
        {
            BindingOperations.EnableCollectionSynchronization(LogItems, _locker);
            _name = name;
            _configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (_configuration.EventId == 0 || _configuration.EventId == eventId.Id)
            {
                LogItem logItem = new()
                {
                    ClassName = _configuration.AddClassName ? _name : null,
                    EventId = _configuration.AddEventId ? eventId : null,
                    DateTime = _configuration.AddDateTime ? DateTime.Now : null,
                    LogLevel = logLevel,
                    Message = formatter(state, exception)
                };

                lock (_locker)
                {
                    LogItems.Add(logItem);
                }
            }
        }
    }
}
