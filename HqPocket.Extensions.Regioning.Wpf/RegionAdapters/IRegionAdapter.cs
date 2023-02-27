using System.Collections;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

public interface IRegionAdapter
{
    FrameworkElement Region { get; }
    object? CurrentView { get; }
    IList Views { get; }
    bool CanNavigate { get; }
    void AddView(object view);
    void RemoveView(string viewName);
    void NavigateTo(string viewName);
}