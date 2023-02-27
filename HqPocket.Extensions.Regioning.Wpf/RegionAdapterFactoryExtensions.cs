namespace HqPocket.Extensions.Regioning;

public static class RegionAdapterFactoryExtensions
{
    public static void AddDefaultRegionAdapters(this IRegionAdapterFactory regionAdapterFactory)
    {
        regionAdapterFactory.AddAdapter<SelectorRegionAdapter>();
        regionAdapterFactory.AddAdapter<ItemsControlRegionAdapter>();
        regionAdapterFactory.AddAdapter<ContentControlRegionAdapter>();
        regionAdapterFactory.AddAdapter<PanelRegionAdapter>();
    }
}