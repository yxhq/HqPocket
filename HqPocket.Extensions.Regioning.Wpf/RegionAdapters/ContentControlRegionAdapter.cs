using System.Collections;
using System.Windows.Controls;

namespace HqPocket.Extensions.Regioning;

public class ContentControlRegionAdapter : RegionAdapterBase<ContentControl>
{
    private readonly ItemCollection _views = new ItemsControl().Items;
    public override IList Views => _views;
    public override object? CurrentView => Region.Content;

    public ContentControlRegionAdapter(ContentControl region) : base(region, true)
    {
        if (region.Content is not null)
        {
            ViewCollectionHelper.AddViewTo(Views, region.Content);
        }
    }

    public override void AddView(object view)
    {
        ViewCollectionHelper.AddViewTo(Views, view);
        Region.Content = view;
    }

    public override void RemoveView(string viewName)
    {
        if (Region.Content.GetType().Name == viewName)
        {
            Region.Content = ViewCollectionHelper.GetFirstView(Views);
        }
        ViewCollectionHelper.RemoveViewFrom(Views, viewName);
    }

    public override void NavigateTo(string viewName)
    {
        var view = ViewCollectionHelper.GetFirstView(Views, viewName);
        if (view is not null)
        {
            Region.Content = view;
        }
    }
}