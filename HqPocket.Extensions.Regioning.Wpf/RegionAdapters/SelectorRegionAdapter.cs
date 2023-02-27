using System.Collections;
using System.Windows.Controls.Primitives;

namespace HqPocket.Extensions.Regioning;

public class SelectorRegionAdapter : RegionAdapterBase<Selector>
{
    public override IList Views => Region.Items;
    public override object? CurrentView => Region.SelectedItem;

    public SelectorRegionAdapter(Selector region) : base(region, true)
    {

    }

    public override void AddView(object view)
    {
        ViewCollectionHelper.AddViewTo(Views, view);
    }

    public override void RemoveView(string viewName)
    {
        if (Region.SelectedItem?.GetType().Name == viewName)
        {
            Region.SelectedItem = ViewCollectionHelper.GetFirstView(Views);
        }
        ViewCollectionHelper.RemoveViewFrom(Views, viewName);
    }

    public override void NavigateTo(string viewName)
    {
        var view = ViewCollectionHelper.GetFirstView(Views, viewName);
        if (view is not null)
        {
            Region.SelectedItem = view;
        }
    }
}