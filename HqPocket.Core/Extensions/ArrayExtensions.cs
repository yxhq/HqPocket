using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable CheckNamespace

namespace System;

public static class ArrayExtensions
{
    public static IEnumerable<T?> GetColumn<T>(this T?[][] rectarray, int column)
    {
        if (column < 0 || column >= rectarray.Max(array => array.Length))
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        for (int r = 0; r < rectarray.GetLength(0); r++)
        {
            if (column >= rectarray[r].Length)
            {
                yield return default;

                continue;
            }

            yield return rectarray[r][column];
        }
    }

    /// <summary>
    /// Returns a simple string representation of an array.
    /// </summary>
    /// <typeparam name="T">The element type of the array.</typeparam>
    /// <param name="array">The source array.</param>
    /// <param name="format">The element format.</param>
    /// <param name="separator">The element separator.</param>
    /// <returns>The <see cref="string"/> representation of the array.</returns>
    public static string ToArrayString<T>(this T?[] array, string format = "{0}", string separator = " ")
    {
        // The returned string will be in the following format:
        // 1 2 3
        if (array.Length == 0) return string.Empty;

        StringBuilder builder = new(string.Format(format, array[0]));
        for (int i = 1; i < array.Length; i++)
        {
            builder.Append(separator);
            builder.AppendFormat(format, array[i]);
        }
        return builder.ToString();
    }
}
