using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace HqPocket.Wpf.MarkupExtensions;

[MarkupExtensionReturnType(typeof(IEnumerable))]
public class EnumValuesExtension : MarkupExtension
{
    [ConstructorArgument("enumType")]
    public Type? EnumType { get; set; }

    [DefaultValue(null)]
    public IValueConverter? Converter { get; set; }

    [DefaultValue(BindingMode.Default)]
    public BindingMode Mode { get; set; }

    [DefaultValue(null)]
    public string? HideValueIndexes { get; set; }

    public EnumValuesExtension()
    {
    }

    public EnumValuesExtension(Type enumType)
    {
        EnumType = enumType;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (EnumType is null) return null;

        Array values = Enum.GetValues(EnumType);
        if (string.IsNullOrEmpty(HideValueIndexes))
        {
            return CreateBinding(values).ProvideValue(serviceProvider);
        }

        var hideValueIndexes = HideValueIndexes.Split(',', ' ').Select(i => int.Parse(i));
        var indexes = Enumerable.Range(0, values.Length).Except(hideValueIndexes).ToArray();
        var newValues = Array.CreateInstance(EnumType, indexes.Length);

        for (int i = 0; i < indexes.Length; i++)
        {
            var v = values.GetValue(indexes[i]);
            if (v is not null)
            {
                newValues.SetValue(v, i);
            }
        }

        return CreateBinding(newValues).ProvideValue(serviceProvider);
    }

    private Binding CreateBinding(object source)
    {
        return new Binding(".")
        {
            Source = source,
            Converter = Converter,
            Mode = BindingMode.OneWay
        };
    }
}