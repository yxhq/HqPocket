using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace HqPocket.Extensions.Localization;

/// <summary>
/// enum 本地化，本地化文件Key为 EnumName_EnumMemberName
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public class LocalizedEnum<TEnum> : DependencyObject where TEnum : struct, Enum
{
    private static IEnumerable<LocalizedEnum<TEnum>>? _localizedEnums;
    public TEnum Value { get; set; }

    public static readonly DependencyProperty LocalizedNameProperty =
        DependencyProperty.Register(nameof(LocalizedName), typeof(string), typeof(LocalizedEnum<TEnum>), new PropertyMetadata(string.Empty));
    public string LocalizedName
    {
        get => (string)GetValue(LocalizedNameProperty);
        set => SetValue(LocalizedNameProperty, value);
    }

    private LocalizedEnum(TEnum value)
    {
        Value = value;
    }

    private LocalizedEnum(TEnum value, Type resourceType)
    {
        Value = value;
        Localizer.Bind(resourceType, this, LocalizedNameProperty, $"{typeof(TEnum).Name}_{value}");
    }

    public static IEnumerable<LocalizedEnum<TEnum>> CreateLocalizedEnumValues(Type resourceType)
    {
         _localizedEnums = Enum.GetValues<TEnum>().Select(v => new LocalizedEnum<TEnum>(v, resourceType));
        return _localizedEnums;
    }

    public static IEnumerable<LocalizedEnum<TEnum>> CreateLocalizedEnumValues<TResource>() => CreateLocalizedEnumValues(typeof(TResource));


    public static implicit operator TEnum(LocalizedEnum<TEnum> localizedEnum) => localizedEnum.Value;


    public static implicit operator LocalizedEnum<TEnum>(TEnum @enum)
    {
        return _localizedEnums is null ? new(@enum) : _localizedEnums.Single(e => e.Value.ToString() == @enum.ToString());
    }

    public override string? ToString() => LocalizedName;
}

