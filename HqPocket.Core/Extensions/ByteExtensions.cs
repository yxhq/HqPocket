using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable CheckNamespace

namespace System;

public static class ByteExtensions
{
    public static string ToHexString(this byte b) => $"{b:X2}";

    public static string ToHexString(this IEnumerable<byte> bs, string separator = " ")
    {           
        return string.Join(separator, bs.Select(b => $"{b:X2}"));
    }

    public static string ToHexString(this Span<byte> bs, string separator = " ")
    {
        if (bs.Length == 0) return string.Empty;

        StringBuilder builder = new($"{bs[0]:X2}");
        for (int i = 1; i < bs.Length; i++)
        {
            builder.Append(separator);
            builder.Append($"{bs[i]:X2}");
        }
        return builder.ToString();
    }
}
