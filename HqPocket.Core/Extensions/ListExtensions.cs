namespace System.Collections.Generic;

public static class ListExtensions
{
    public static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (predicate(item))
            {
                list.RemoveAt(i);
            }
        }
    }
}
