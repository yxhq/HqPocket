using HqPocket.Wpf.Helpers;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace HqPocket.Extensions.Localization;

public class ViewLocalizerExtension : MarkupExtension
{
    private readonly DependencyObject _proxy;
    public BindingMode Mode { get; set; } = BindingMode.OneWay;
    public IValueConverter? Converter { get; set; }
    public object? ConverterParameter { get; set; }
    public Type? ResourceType { get; set; }

    public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
        "Key", typeof(object), typeof(ViewLocalizerExtension), new PropertyMetadata(default));

    [ConstructorArgument("key")]
    public object? Key
    {
        get => _proxy.GetValue(KeyProperty);
        set => _proxy.SetValue(KeyProperty, value);
    }

    public ViewLocalizerExtension(object key) : this()
    {
        Key = key;
    }

    public ViewLocalizerExtension()
    {
        _proxy = new DependencyObject();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Key is null) return string.Empty;

        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget service) return this;
        if (service.TargetObject.GetType().FullName == "System.Windows.SharedDp") return this;
        if (service.TargetObject is not DependencyObject targetObject) return this;
        if (service.TargetProperty is not DependencyProperty) return this;

        if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) return Key;

        var bindingKey = Key switch
        {
            string key => key,
            Binding keyBinding when targetObject is FrameworkElement element => BindingHelper.GetBindingValue(element.DataContext, keyBinding.Path),
            Binding keyBinding when targetObject is FrameworkContentElement element => BindingHelper.GetBindingValue(element.DataContext, keyBinding.Path),
            _ => null
        };
        if (bindingKey is null) return string.Empty;

        var resourceType = ResourceType switch
        {
            Type type => type,
            null when serviceProvider is IRootObjectProvider { RootObject: DependencyObject dpObj } => dpObj.DependencyObjectType.SystemType,
            null when targetObject is FrameworkElement element => element.TryFindParent<UserControl>()?.GetType(),
            null when targetObject is FrameworkContentElement element => element.TryFindParent<UserControl>()?.GetType(),
            _ => null
        };

        if (resourceType is not null)
        {
            var localizerFactory = Ioc.GetRequiredService<INotifiedStringLocalizerFactory>();
            var localizer = localizerFactory.Create(resourceType);
            if (localizer[$"{bindingKey}"].ResourceNotFound)
            {
                var assembly = resourceType.Assembly;
                var sharedResourceTypeName = $"{assembly.GetName().Name}.{Conventions.ResourceDirectory}.{Conventions.SharedResourceName}";
                resourceType = assembly.GetType(sharedResourceTypeName);
                if (resourceType is not null)
                {
                    localizer = localizerFactory.Create(resourceType);
                }
            }

            Binding binding = new($"[{bindingKey}].Value")
            {
                Source = localizer,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                Mode = Mode,
                Converter = Converter,
                ConverterParameter = ConverterParameter
            };

            return binding.ProvideValue(serviceProvider);
        }

        return $"[{bindingKey}]";
    }
}
