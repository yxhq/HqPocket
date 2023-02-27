using System.Windows;

namespace HqPocket.Extensions.Regioning;

public interface IRegionAdapterFactory
{
    void AddAdapter<TAdapter>() where TAdapter : IRegionAdapter;
    IRegionAdapter Create(FrameworkElement element);
}