using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace HqPocket.Extensions.Localization
{
    public class NotifiedResourceManagerStringLocalizerFactory : ResourceManagerStringLocalizerFactory, INotifiedStringLocalizerFactory
    {
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentBag<NotifiedResourceManagerStringLocalizer> _localizerCache = new();
        public NotifiedResourceManagerStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
            : base(localizationOptions, loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        protected override NotifiedResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, string baseName)
        {

            var localizer = new NotifiedResourceManagerStringLocalizer(new ResourceManager(baseName, assembly), assembly, baseName,
                _resourceNamesCache, _loggerFactory.CreateLogger<NotifiedResourceManagerStringLocalizer>());

            if (!_localizerCache.Contains(localizer))
            {
                _localizerCache.Add(localizer);
            }
            return localizer;
        }

        public void RefreshLocalizedString()
        {
            foreach (var localizer in _localizerCache)
            {
                localizer.RaiseItemPropertyChanged();
            }
        }
    }
}
