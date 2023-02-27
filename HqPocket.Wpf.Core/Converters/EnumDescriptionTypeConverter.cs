using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HqPocket.Wpf.Converters;

public class EnumDescriptionTypeConverter : EnumConverter
{
    public EnumDescriptionTypeConverter(Type type) : base(type)
    {
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            var fieldInfo = value?.GetType().GetField(value.ToString()!);
            if (fieldInfo is not null)
            {
                var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false).ToArray();
                return attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description)
                    ? attributes[0].Description : value?.ToString();
            }
            return string.Empty;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}