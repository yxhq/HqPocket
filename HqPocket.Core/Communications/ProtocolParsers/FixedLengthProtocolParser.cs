using System;

namespace HqPocket.Communications;

public class FixedLengthProtocolParser : ProtocolParserBase<FixedLengthProtocol>
{
    protected override int HeadToDataLength { get; }

    public FixedLengthProtocolParser(FixedLengthProtocol protocol)
        : base(protocol)
    {
        Initialize();
        HeadToDataLength = HeadLength + (Protocol.Data?.Length ?? 0);
    }

    public override void Parse(ReadOnlySpan<byte> bytes, Action<FixedLengthProtocol>? succeedCallback, Action<FixedLengthProtocol>? failedCallback)
    {
        foreach (var buf in bytes)
        {
            Index++;

            if (Index < HeadLength)
            {
                if (buf != Protocol.Head[Index])
                {
                    Index = -1;
                }
            }
            else if (Index < HeadToDataLength)
            {
                Protocol.Data![Index - HeadLength] = buf;
            }
            else if (Index < HeadToCheckBytesLength)    // 仅在 Has CheckBytes 时运行
            {
                Protocol.CheckBytes![Index - HeadToDataLength] = buf;
            }
            else if (Index < HeadToTailLength)  // 仅在 Has Tail 时运行
            {
                if (Protocol.Tail![Index - HeadToCheckBytesLength] != buf)
                {
                    ParseNoError = false;
                }
            }

            if (Index == HeadToTailLength - 1)
            {
                Index = -1;

                ParseNoError = ParseNoError && (!Protocol.HasCheckBytes || (Protocol.HasCheckBytes && Protocol.IsCheckedCorrect()));

                if (ParseNoError)
                {
                    succeedCallback?.Invoke(Protocol);
                }
                else
                {
                    failedCallback?.Invoke(Protocol);
                }

                ParseNoError = true;
            }
        }
    }
}
