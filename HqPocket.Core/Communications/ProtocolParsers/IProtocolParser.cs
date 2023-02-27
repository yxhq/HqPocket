using System;

namespace HqPocket.Communications;


public interface IProtocolParser<TProtocol> where TProtocol : IProtocol
{
    TProtocol Protocol { get; set; }
    void Initialize();
    void Parse(ReadOnlySpan<byte> bytes, Action<TProtocol>? succeedCallback, Action<TProtocol>? failedCallback);
}
