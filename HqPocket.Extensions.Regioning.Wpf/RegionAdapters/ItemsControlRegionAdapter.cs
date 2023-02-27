using System.Collections;
using System.Windows.Controls;

namespace HqPocket.Extensions.Regioning;

public class ItemsControlRegionAdapter : RegionAdapterBase<ItemsControl>
{
    public override IList Views => Region.Items;

    public ItemsControlRegionAdapter(ItemsControl region) : base(region, false)
    {

    }

    public override void AddView(object view)
    {
        ViewCollectionHelper.AddViewTo(Views, view);
    }

    public override void RemoveView(string viewName)
    {
        ViewCollectionHelper.RemoveViewFrom(Views, viewName);
    }
}