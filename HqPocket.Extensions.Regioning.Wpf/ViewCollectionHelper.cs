using System;
using System.Collections;
using System.Linq;

namespace HqPocket.Extensions.Regioning;

public static class ViewCollectionHelper
{
    public static void AddViewTo(IList list, object view)
    {
        int index = Array.BinarySearch(list.Cast<object>().ToArray(), view, new ViewSortComparer());
        if (index < 0) index = ~index;
        list.Insert(index, view);
    }

    public static void RemoveViewFrom(IList list, string viewName)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (item is not null && item.GetType().Name == viewName)
            {
                list.RemoveAt(i);
            }
        }
    }


    public static object? GetFirstView(IList? views, string? viewName = null)
    {
        if (views is null || views.Count == 0) return null;

        if (string.IsNullOrWhiteSpace(viewName))
        {
            return views[0];
        }

        foreach (var view in views)
        {
            if (view.GetType().Name == viewName)
            {
                return view;
            }
        }
        return null;
    }
}