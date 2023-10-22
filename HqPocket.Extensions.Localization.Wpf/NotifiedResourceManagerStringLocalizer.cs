using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace HqPocket.Extensions.Localization;

public class NotifiedResourceManagerStringLocalizer : ResourceManagerStringLocalizer, INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs _indexerPropertyChanged = new("Item[]");

    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaiseItemPropertyChanged() => PropertyChanged?.Invoke(this, _indexerPropertyChanged);

    public NotifiedResourceManagerStringLocalizer(ResourceManager resourceManager, Assembly resourceAssembly, string baseName, IResourceNamesCache resourceNamesCache, ILogger logger)
        : base(resourceManager, resourceAssembly, baseName, resourceNamesCache, logger)
    {
    }
}
