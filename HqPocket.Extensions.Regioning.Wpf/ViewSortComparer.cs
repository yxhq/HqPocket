using System;
using System.Collections;
using System.Reflection;

namespace HqPocket.Extensions.Regioning;

/// <summary>
/// 按ViewSortHint排序，没有该特性的放于最后
/// </summary>
public class ViewSortComparer : IComparer
{
    public int Compare(object? x, object? y)
    {
        if (x is null)
        {
            if (y is null)
            {
                return 0;
            }
            return 1;
        }

        if (y is null)
        {
            return -1;
        }

        Type xType = x.GetType();
        Type yType = y.GetType();

        ViewSortHintAttribute? xAttribute = xType.GetCustomAttribute<ViewSortHintAttribute>();
        ViewSortHintAttribute? yAttribute = yType.GetCustomAttribute<ViewSortHintAttribute>();
        return ViewSortHintAttributeComparison(xAttribute, yAttribute);

    }

    private static int ViewSortHintAttributeComparison(ViewSortHintAttribute? x, ViewSortHintAttribute? y)
    {
        if (x is null)
        {
            if (y is null)
            {
                //return 0;
                return -1; //x，y都没有该属性时，加到后面，若返回0，则会插到第一个位置
            }
            return 1;
        }

        if (y is null)
        {
            return -1;
        }

        return string.Compare(x.Hint, y.Hint, StringComparison.Ordinal);
    }
}