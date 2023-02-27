using System.Text.RegularExpressions;

namespace HqPocket.Helpers;

/// <summary>
/// 字符串格式化帮助类对"{index[,alignment][:formatString]}"进行解析
/// </summary>
public class FormatHelper
{
    /// <summary>
    /// 去掉 alignment 后剩余部分
    /// </summary>
    /// <param name="format">"{index[,alignment][:formatString]}"</param>
    /// <returns>"{index[:formatString]}"</returns>
    public static string GetFormatWithoutAlignment(string format)
    {
        return Regex.Replace(format, @",-{0,1}\d+(?=:)", "");
    }

    /// <summary>
    /// 提取 foformatString
    /// </summary>
    /// <param name="format">"{index[,alignment][:formatString]}"</param>
    /// <returns>formatString</returns>
    public static string GetFormatString(string format)
    {
        return Regex.Match(format, @"(?<=:).*(?=})").Value;
    }

    /// <summary>
    /// 提取 alignment
    /// </summary>
    /// <param name="format">"{index[,alignment][:formatString]}"</param>
    /// <returns>alignment</returns>
    public static int GetAlignment(string format)
    {
        var match = Regex.Match(format, @"(?<=,)-{0,1}\d+(?=:)");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    /// <summary>
    /// 提取浮点数小数位数
    /// </summary>
    /// <param name="format">"{index[,alignment][:formatString]}"</param>
    /// <returns>formatString中的数字</returns>
    public static int GetDigits(string format)
    {
        var match = Regex.Match(format, @"(?<=:[fF]{1})\d+(?=})");
        return match.Success ? int.Parse(match.Value) : 0;
    }
}
