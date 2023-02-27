using System.Collections.Concurrent;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

public class RegionManagerImpl : IRegionManager
{
    private readonly IRegionAdapterFactory _regionAdapterFactory;
    private static readonly ConcurrentDictionary<string, Region> _regions = new();

    public RegionManagerImpl(IRegionAdapterFactory regionAdapterFactory)
    {
        _regionAdapterFactory = regionAdapterFactory;
    }

    internal static void InitializeRegion(string regionName, FrameworkElement frameworkElement)
    {
        if (!_regions.ContainsKey(regionName)) return;
        var region = _regions[regionName];
        region.Initialize(frameworkElement);
    }

    public IRegion GetRegion(string regionName)
    {
        return _regions.GetOrAdd(regionName, new Region(regionName, _regionAdapterFactory));
    }
}