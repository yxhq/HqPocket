using System.Collections.Generic;

namespace HqPocket.Extensions.Communications;

public interface ICheckMethod
{
    string Name { get; set; }
    int CheckByteCount { get; set; }
    ByteOrder CheckByteOrder { get; set; }
    IEnumerable<byte> Check(IEnumerable<byte> bytes);
}
