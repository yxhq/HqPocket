using HqPocket.Helpers;
using System.Reflection;

namespace HqPocket.IO;

public class ReadWriteItem
{
    public PropertyInfo PropertyInfo { get; }
    public string DisplayName { get; }
    public bool CanRead { get; }
    /// <summary>
    /// 从 0 开始
    /// </summary>
    public int Column { get; }
    public string Format { get; }

    public ReadWriteItem(PropertyInfo propertyInfo, string? displayName, bool isAlign, ReadWriteAttribute readWriteAttribute)
    {
        PropertyInfo = propertyInfo;
        DisplayName = displayName ?? propertyInfo.Name;
        CanRead = readWriteAttribute.CanRead;
        Column = readWriteAttribute.Column;

        string format = string.IsNullOrWhiteSpace(readWriteAttribute.Format) ? "{0}" : readWriteAttribute.Format;
        Format = isAlign ? format : FormatHelper.GetFormatWithoutAlignment(format);
    }
}