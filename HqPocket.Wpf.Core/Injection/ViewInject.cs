using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HqPocket.Wpf.Injection;

public class ViewInject : ContentPresenter
{
    public static readonly DependencyProperty ViewTypeProperty =
        DependencyProperty.Register(nameof(ViewType), typeof(Type), typeof(ViewInject),
            new UIPropertyMetadata(OnViewTypeChanged));

    private static void OnViewTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d)) return;

        if (d is ContentPresenter contentPresenter && e.NewValue is Type viewType)
        {
            contentPresenter.Content = Ioc.GetRequiredService(viewType);
        }
    }

    public Type ViewType
    {
        get => (Type)GetValue(ViewTypeProperty);
        set => SetValue(ViewTypeProperty, value);
    }


    public static readonly DependencyProperty ViewModelTypeProperty =
        DependencyProperty.Register(nameof(ViewModelType), typeof(Type), typeof(ViewInject),
            new UIPropertyMetadata(OnViewModelTypeChanged));

    private static void OnViewModelTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d)) return;

        if (d is ContentPresenter { Content: FrameworkElement element } contentPresenter && e.NewValue is Type viewModelType)
        {
            contentPresenter.DataContext = element.DataContext = Ioc.GetRequiredService(viewModelType);
            //ViewModelHelper.SetViewForViewModel(viewModelType, element);
        }
    }

    public Type ViewModelType
    {
        get => (Type)GetValue(ViewModelTypeProperty);
        set => SetValue(ViewModelTypeProperty, value);
    }
}