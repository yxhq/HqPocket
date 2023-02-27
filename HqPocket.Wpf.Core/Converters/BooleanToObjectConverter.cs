using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Wpf.Converters;

[ValueConversion(typeof(bool), typeof(object))]
public class BooleanToObjectConverter : IValueConverter
{
    public object? TrueObject { get; set; }
    public object? FalseObject { get; set; }
    public object? NullObject { get; set; }

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            null => NullObject,
            bool b => b ? TrueObject : FalseObject,
            _ => DependencyProperty.UnsetValue,
        };
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}