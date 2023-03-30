using System;

namespace HqPocket.Communications;

public class VariableLengthProtocolParser : ProtocolParserBase<VariableLengthProtocol>
{
    protected override int HeadToDataLength => HeadLength + 1 + Protocol.DataLength;

    public VariableLengthProtocolParser(VariableLengthProtocol protocol) : base(protocol)
    {
        Initialize();
    }

    public override void Parse(ReadOnlySpan<byte> bytes, Action<VariableLengthProtocol>? succeedCallback, Action<VariableLengthProtocol>? failedCallback)
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
                Protocol.DataLength = 0;
            }
            else if (Index == HeadLength) // Protocol.DataLength 字节
            {
                if (buf != Protocol.Head[HeadLength - 1])
                {
                    Protocol.DataLength = buf;
                    if (buf > 0 && (Protocol.Data is null || buf != Protocol.Data.Length))
                    {
                        Protocol.Data = new byte[buf];
                    }
                }
                else
                {
                    Index--;
                }
            }
            else if (Index < HeadToDataLength)
            {
                if (Protocol.DataLength > 0)
                {
                    Protocol.Data![Index - HeadLength - 1] = buf;
                }
            }
            else if (Index < HeadToCheckBytesLength && Protocol.HasCheckBytes) // 仅在 Has CheckBytes 时运行
            {
                Protocol.CheckBytes![Index - HeadToDataLength] = buf;
            }
            else if (Index < HeadToTailLength && Protocol.HasTail)  // 仅在 Has Tail 时运行
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
