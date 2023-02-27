using System.Collections.Generic;
// ReSharper disable CheckNamespace

namespace System.Linq;

public static partial class LinqExtensions
{
    public static int FirstIndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        int index = 0;
        foreach (var item in source)
        {
            if (predicate(item))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    public static int LastIndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        int index = source.Count();
        foreach (var item in source.Reverse())
        {
            index--;
            if (predicate(item))
            {
                return index;
            }
        }
        return -1;
    }
}
