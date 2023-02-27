using System;
using System.Globalization;
using System.IO.Ports;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Pockets.Converters;

[ValueConversion(typeof(StopBits), typeof(double))]
public class SerialPortStopBitsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is StopBits stopBits)
        {
            return stopBits switch
            {
                StopBits.None => 0,
                StopBits.One => 1,
                StopBits.OnePointFive => 1.5,
                StopBits.Two => 2,
                _ => 0
            };
        }
        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double stopBits)
        {
            return stopBits switch
            {
                0 => StopBits.None,
                1 => StopBits.One,
                1.5 => StopBits.OnePointFive,
                2 => StopBits.Two,
                _ => StopBits.None
            };
        }
        return DependencyProperty.UnsetValue;
    }
}
