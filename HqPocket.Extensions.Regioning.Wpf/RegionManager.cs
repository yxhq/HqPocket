
using System.ComponentModel;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

public class RegionManager
{
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager),
            new FrameworkPropertyMetadata(RegionNamePropertyChanged));

    public static string GetRegionName(DependencyObject obj)
    {
        return (string)obj.GetValue(RegionNameProperty);
    }

    public static void SetRegionName(DependencyObject obj, string value)
    {
        obj.SetValue(RegionNameProperty, value);
    }

    private static void RegionNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d)) return;

        if (d is FrameworkElement frameworkElement && e.NewValue is string regionName)
        {
            RegionManagerImpl.InitializeRegion(regionName, frameworkElement);
        }
    }
}