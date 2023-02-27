using System;

namespace HqPocket.Extensions.Regioning;

public static class RegionExtensions
{
    public static IRegion AddView<TView>(this IRegion region)
    {
        return region.AddView(typeof(TView));
    }

    public static void RemoveView(this IRegion region, Type viewType)
    {
        region.RemoveView(viewType.Name);
    }

    public static void RemoveView<TView>(this IRegion region)
    {
        region.RemoveView(typeof(TView));
    }


    public static void NavigateTo(this IRegion region, Type viewType)
    {
        region.NavigateTo(viewType.Name);
    }

    public static void NavigateTo<TView>(this IRegion region)
    {
        region.NavigateTo(typeof(TView).Name);
    }
}