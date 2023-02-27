using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HqPocket.Comparers;

public class LetterNumberComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }
        else
        {
            if (y is null)
            {
                return 1;
            }
            else
            {
                if (!Regex.IsMatch(x, "[0-9]+") || !Regex.IsMatch(y, "[0-9]+"))
                {
                    throw new Exception("字符串中必须包含数字");
                }

                int ix = int.Parse(Regex.Replace(x, @"[^0-9]+", ""));
                int iy = int.Parse(Regex.Replace(y, @"[^0-9]+", ""));
                return ix - iy;
            }
        }
    }
}
