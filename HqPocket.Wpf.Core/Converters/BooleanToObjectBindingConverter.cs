using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Wpf.Converters;

/// <summary>
/// MultiValueConverter，包含3个值，[0]-bool，[1]-true返回值，[2]-false返回值
/// Example:
/// <Button.Content>
/// <MultiBinding Converter = "{StaticResource BooleanToObjectBindingConverter}" NotifyOnSourceUpdated="True">
///     <Binding Path = "Boolean"/>
///     <Binding Path = "TrueContent" RelativeSource="{RelativeSource Mode=FindAncestor, AncestorType=AncestorType}"/>
///     <Binding Path = "FalseContent" RelativeSource="{RelativeSource Mode=FindAncestor, AncestorType=AncestorType}"/>
/// </MultiBinding>
/// </Button.Content>
/// </summary>
[ValueConversion(typeof(object[]), typeof(object))]
public class BooleanToObjectBindingConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 3)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        if (values[0] is bool b)
        {
            return b ? values[1] : values[2];
        }

        return DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return new[] { DependencyProperty.UnsetValue };
    }
}