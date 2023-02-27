using System.Windows;
using System.Windows.Data;

namespace HqPocket.Wpf.Helpers;

public static class BindingHelper
{
    private static readonly Dummy _dummy = new();
    public static object? GetBindingValue(object? obj, PropertyPath propertyPath)
    {
        if (obj is null) return null;

        Binding binding = new()
        {
            Path = propertyPath,
            Mode = BindingMode.OneTime,
            Source = obj
        };
        BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
        return _dummy.GetValue(Dummy.ValueProperty);
    }

    private class Dummy : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(object), typeof(Dummy), new UIPropertyMetadata(default));
    }
}