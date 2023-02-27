using HqPocket.Helpers;
using HqPocket.RegularExpressions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace

namespace System;

public static class StringExtensions
{
    /// <summary>
    /// 文本对齐，中英混合
    /// </summary>
    /// <param name="text"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string Align(this string text, string format)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        int textLength = Encoding.GetEncoding("gb2312").GetBytes(text).Length;

        var alignment = FormatHelper.GetAlignment(format);
        if (alignment > 0)//右对齐
        {
            StringBuilder stringBuilder = new(new string(' ', Math.Abs(alignment - textLength)));
            stringBuilder.Append(text);
            return stringBuilder.ToString();
        }

        if (alignment < 0)//左对齐
        {
            StringBuilder stringBuilder = new(text);
            stringBuilder.Append(new string(' ', Math.Abs(alignment + textLength)).AsSpan());
            return stringBuilder.ToString();
        }

        return string.Format(format, text);
    }

    public static byte[] ToByteArray(this string hexString, string separator = " ")
    {
        if (!Regex.IsMatch(hexString, RegularPatterns.HexWithSeparator(separator)))
        {
            throw new Exception($"字符串格式错误，应为用“{separator}”隔开的十六进制数据");
        }

        return hexString.Split(separator).Select(s => Convert.ToByte(s, 16)).ToArray();
    }


    //public static byte[] ToByteArray(this string hexString, string separator = " ")
    //{
    //    Regex regex = new($"^[0-9A-Fa-f{separator}]+$");
    //    if (!regex.IsMatch(hexString))
    //    {
    //        throw new Exception($"字符串格式错误，应为连续的十六进制数据或者用“{separator}”隔开");
    //    }

    //    List<string> result = new();
    //    var hexStringSpan = hexString.Trim(separator.ToCharArray()).AsSpan();

    //    int separatorLength = separator.Length;

    //    void AddSlice(ReadOnlySpan<char> hexStr)
    //    {
    //        if (hexStr.Length == 1)
    //        {
    //            result.Add(hexStr.ToString());
    //        }
    //        else
    //        {
    //            int count = hexStr.Length / 2;
    //            for (int i = 0; i < count; i++)
    //            {
    //                result.Add(hexStr.Slice(i * 2, 2).ToString());
    //            }
    //        }
    //    }

    //    while (true)
    //    {
    //        var indexOfSeparator = hexStringSpan.IndexOf(separator);
    //        if (indexOfSeparator > 0)
    //        {
    //            AddSlice(hexStringSpan[..indexOfSeparator]);
    //            hexStringSpan = hexStringSpan[(indexOfSeparator + separatorLength)..];
    //        }
    //        else
    //        {
    //            AddSlice(hexStringSpan);
    //            break;
    //        }
    //    }
    //    return (from s in result select Convert.ToByte(s, 16)).ToArray();
    //}

    public static string ExtractAllHexString(this string hexString, string separator = " ")
    {
        if (hexString is null) return string.Empty;

        var matches = Regex.Matches(hexString, RegularPatterns.Hex);
        if (matches.Count > 0)
        {
            return string.Join(separator, matches.Select(m => m.Value));
        }
        return string.Empty;
    }
}
