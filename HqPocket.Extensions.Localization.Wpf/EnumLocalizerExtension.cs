using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace HqPocket.Extensions.Localization;

public class EnumLocalizerExtension : MarkupExtension
{
    public Type? EnumType { get; set; }

    public Type? ResourceType { get; set; }


    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(EnumType);

        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget service) return this;
        if (service.TargetObject.GetType().FullName == "System.Windows.SharedDp") return this;
        if (service.TargetObject is not DependencyObject targetObject) return this;
        if (service.TargetProperty is not DependencyProperty) return this;

        if (DesignerProperties.GetIsInDesignMode(new())) return Enum.GetValues(EnumType);

        var localizedEnumType = typeof(LocalizedEnum<>).MakeGenericType(EnumType);
        var method = localizedEnumType.GetMethods().SingleOrDefault(m => m.Name == "CreateLocalizedEnumValues" && !m.IsGenericMethod);

        var resourceType = ResourceType switch
        {
            Type type => type,
            null when serviceProvider is IRootObjectProvider { RootObject: DependencyObject dpObj } => dpObj.DependencyObjectType.SystemType,
            null when targetObject is FrameworkElement element => element.TryFindParent<UserControl>()?.GetType(),
            null when targetObject is FrameworkContentElement element => element.TryFindParent<UserControl>()?.GetType(),
            _ => null
        };

        if (resourceType is null)
        {
            var assembly = targetObject.GetType().Assembly;
            var sharedResourceTypeName = $"{assembly.GetName().Name}.{Conventions.ResourceDirectory}.{Conventions.SharedResourceName}";
            resourceType = assembly.GetType(sharedResourceTypeName);
        }

        ArgumentNullException.ThrowIfNull(resourceType);

        return method?.Invoke(null, new Type[] { resourceType });
    }
}
