using System;
using System.Globalization;
using System.IO.Ports;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Pockets.Converters;

[ValueConversion(typeof(StopBits[]), typeof(double[]))]
public class SerialPortStopBitsArrayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is StopBits[] stopBits)
        {
            double[] resultArray = new double[stopBits.Length];

            for (int i = 0; i < stopBits.Length; i++)
            {
                var resultItem = stopBits[i] switch
                {
                    StopBits.None => 0,
                    StopBits.One => 1,
                    StopBits.OnePointFive => 1.5,
                    StopBits.Two => 2,
                    _ => 0
                };
                resultArray[i] = resultItem;
            }
            return resultArray;
        }
        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}
