using System;
using System.Globalization;
using System.Windows.Data;

namespace HqPocket.Wpf.Converters;

[ValueConversion(typeof(double), typeof(string))]
public class ZeroToStringEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return double.TryParse(value.ToString(), out double d) && d == 0 ? string.Empty : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace(value.ToString()) ? 0 : value;
    }
}