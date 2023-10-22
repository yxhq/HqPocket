using System.Collections.Generic;

namespace HqPocket.Extensions.Communications;

public class BBCCheckMethod : CheckMethodBase
{
    public BBCCheckMethod() : base("BBC异或校验", 1)
    {
    }

    public override IEnumerable<byte> Check(IEnumerable<byte> bytes)
    {
        byte chk = 0x00;
        foreach (var b in bytes)
        {
            chk ^= b;
        }
        yield return chk;
    }
}
