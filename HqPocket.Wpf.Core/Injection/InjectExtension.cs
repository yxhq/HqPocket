using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace HqPocket.Wpf.Injection;

[MarkupExtensionReturnType(typeof(object))]
public class InjectExtension : MarkupExtension
{
    public Type? ServiceType { get; set; }
    public Type? GenericTypeArgument1 { get; set; }
    public Type? GenericTypeArgument2 { get; set; }
    public Type? GenericTypeArgument3 { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget || ServiceType is null) return this;

        var serviceType = ServiceType;
        if (ServiceType.IsGenericType)
        {
            ArgumentNullException.ThrowIfNull(GenericTypeArgument1);

            List<Type> typeArguments = new();
            if (GenericTypeArgument1 is not null)
            {
                typeArguments.Add(GenericTypeArgument1);
            }
            if (GenericTypeArgument2 is not null)
            {
                typeArguments.Add(GenericTypeArgument2);
            }
            if (GenericTypeArgument3 is not null)
            {
                typeArguments.Add(GenericTypeArgument3);
            }

            serviceType = ServiceType.MakeGenericType(typeArguments.ToArray());
        }
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return serviceType.ToString();
        }
        var result = Ioc.GetService(serviceType);
        return result ?? serviceType.ToString();
    }

}