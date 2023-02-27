using System.Collections;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

public abstract class RegionAdapterBase<TRegion> : IRegionAdapter where TRegion : FrameworkElement
{
    protected TRegion Region { get; }
    FrameworkElement IRegionAdapter.Region => Region;
    public bool CanNavigate { get; }
    public virtual object? CurrentView => null;
    public abstract IList Views { get; }

    protected RegionAdapterBase(TRegion region, bool canNavigate)
    {
        Region = region;
        CanNavigate = canNavigate;
    }

    public abstract void AddView(object view);
    public abstract void RemoveView(string viewName);

    public virtual void NavigateTo(string viewName)
    {

    }
}