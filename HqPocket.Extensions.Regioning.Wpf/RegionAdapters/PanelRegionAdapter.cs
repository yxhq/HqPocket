using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace HqPocket.Extensions.Regioning;

public class PanelRegionAdapter : RegionAdapterBase<Panel>
{
    public override IList Views => Region.Children;

    public PanelRegionAdapter(Panel region) : base(region, false)
    {

    }

    public override void AddView(object view)
    {
        if (view is not UIElement element)
        {
            throw new ArgumentException("View must be UIElement.", nameof(view));
        }

        ViewCollectionHelper.AddViewTo(Views, element);
    }

    public override void RemoveView(string viewName)
    {
        ViewCollectionHelper.RemoveViewFrom(Views, viewName);
    }
}