
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HqPocket.Communications;

public enum ByteOrder
{
    /// <summary>
    /// 高字节在前
    /// </summary>
    [Display(Name = "高字节在前")]
    [Description("高字节在前")]
    BigEndian,
    /// <summary>
    /// 低字节在前
    /// </summary>
    [Display(Name = "低字节在前")]
    [Description("低字节在前")]
    LittleEndian,
}
