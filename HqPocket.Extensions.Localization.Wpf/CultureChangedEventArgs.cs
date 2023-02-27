using System;
using System.Globalization;

namespace HqPocket.Extensions.Localization;

public class CultureChangedEventArgs : EventArgs
{
    public CultureChangedEventArgs(CultureInfo cultureInfo)
    {
        CultureInfo = cultureInfo;
    }
    public CultureInfo CultureInfo { get; set; }
}
