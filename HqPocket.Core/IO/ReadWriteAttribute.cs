
using System;

namespace HqPocket.IO;

// ReSharper disable once RedundantAttributeUsageProperty
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class ReadWriteAttribute : Attribute
{
    /// <summary>
    /// 格式化
    /// </summary>
    public string Format { get; set; }
    /// <summary>
    /// 该项是否可以从文件中读取
    /// </summary>
    public bool CanRead { get; set; }
    /// <summary>
    /// 仅表示相对的列的位置，不是真正的列序号
    /// </summary>
    public int Column { get; set; }

    public ReadWriteAttribute(string format, int column, bool canRead = true)
    {
        Format = format;
        Column = column;
        CanRead = canRead;
    }
}
