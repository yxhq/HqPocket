using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HqPocket.Wpf.Converters;

[ValueConversion(typeof(bool), typeof(object))]
public class BooleanToObjectConverter :DependencyObject, IValueConverter
{

    public static readonly DependencyProperty TrueObjectProperty =
        DependencyProperty.Register(nameof(TrueObject), typeof(object), typeof(BooleanToObjectConverter), new PropertyMetadata(default));

    public object TrueObject
    {
        get => (object)GetValue(TrueObjectProperty);
        set => SetValue(TrueObjectProperty, value);
    }

    public static readonly DependencyProperty FalseObjectProperty =
        DependencyProperty.Register(nameof(FalseObject), typeof(object), typeof(BooleanToObjectConverter), new PropertyMetadata(default));

    public object FalseObject
    {
        get => (object)GetValue(FalseObjectProperty);
        set => SetValue(FalseObjectProperty, value);
    }

    public static readonly DependencyProperty NullObjectProperty =
        DependencyProperty.Register(nameof(NullObjectProperty), typeof(object), typeof(BooleanToObjectConverter), new PropertyMetadata(default));

    public object NullObject
    {
        get => (object)GetValue(NullObjectProperty);
        set => SetValue(NullObjectProperty, value);
    }

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
        if (value is null) return NullObject;
        if (value == TrueObject) return true;
        if (value == FalseObject) return false;

        return DependencyProperty.UnsetValue;
    }
}