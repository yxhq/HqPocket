using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Wpf.Converters;

[ValueConversion(typeof(bool), typeof(object))]
public class BooleanToResourceConverter : IValueConverter
{
    public object? TrueResourceName { get; set; }
    public object? FalseResourceName { get; set; }
    public object? NullResourceName { get; set; }

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            null => Application.Current.TryFindResource(NullResourceName),
            bool b => b ? Application.Current.TryFindResource(TrueResourceName) : Application.Current.TryFindResource(FalseResourceName),
            _ => DependencyProperty.UnsetValue,
        };
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}