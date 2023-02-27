using System.Collections.Generic;

namespace HqPocket.Communications;

public class CSCheckMethod : CheckMethodBase
{
    public CSCheckMethod() : base("CS和校验", 1)
    {
    }

    public override IEnumerable<byte> Check(IEnumerable<byte> bytes)
    {
        byte chk = 0x00;
        foreach (var b in bytes)
        {
            chk += b;
        }

        yield return chk;
    }
}
