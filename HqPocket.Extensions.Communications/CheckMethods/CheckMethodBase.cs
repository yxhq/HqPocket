using System.Collections.Generic;

namespace HqPocket.Extensions.Communications;

public abstract class CheckMethodBase : ICheckMethod
{
    public string Name { get; set; }
    public int CheckByteCount { get; set; }
    public ByteOrder CheckByteOrder { get; set; }

    public abstract IEnumerable<byte> Check(IEnumerable<byte> bytes);

    public CheckMethodBase(string name = "", int checkByteCount = 1, ByteOrder checkByteOrder = ByteOrder.LittleEndian)
    {
        Name = name;
        CheckByteCount = checkByteCount;
        CheckByteOrder = checkByteOrder;
    }
}
