using HqPocket.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;

namespace HqPocket.Wpf.Mvvm;

public static class ViewModelLocator
{
    public static readonly DependencyProperty AutoWireViewModelProperty =
        DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator),
            new PropertyMetadata(false, AutoWireViewModelChanged));

    public static bool? GetAutoWireViewModel(DependencyObject obj)
    {
        return (bool?)obj.GetValue(AutoWireViewModelProperty);
    }

    public static void SetAutoWireViewModel(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoWireViewModelProperty, value);
    }

    private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d)) return;

        if ((bool)e.NewValue && d is FrameworkElement element)
        {
            Type viewModelType = VvmTypeLocationProvider.GetMappedType(element.GetType());
            element.DataContext = Ioc.GetRequiredService(viewModelType);
        }
    }
}