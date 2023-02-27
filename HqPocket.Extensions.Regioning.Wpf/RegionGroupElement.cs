
using System.Collections.ObjectModel;

namespace HqPocket.Extensions.Regioning;

public class RegionGroupElement
{
    public string Title { get; set; }
    public ObservableCollection<IRegionElement> Views { get; } = new();
    public RegionGroupElement(string title)
    {
        Title = title;
    }
}