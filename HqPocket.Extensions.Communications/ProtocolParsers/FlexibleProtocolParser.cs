using System;

namespace HqPocket.Extensions.Communications;

public class FlexibleProtocolParser : ProtocolParserBase<FlexibleProtocol>
{
    protected override int HeadToDataLength => HeadLength + 4 + Protocol.DataLength;

    public FlexibleProtocolParser(FlexibleProtocol protocol) : base(protocol)
    {
    }

    public override void Initialize()
    {
        Index = -1;
    }

    public override void Parse(ReadOnlySpan<byte> bytes, Action<FlexibleProtocol>? succeedCallback, Action<FlexibleProtocol>? failedCallback)
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
            else if (Index == HeadLength)   // 为了更通用，此处不检查TargetId是否正确，若需要可在succeedCallback中处理
            {
                if (buf != Protocol.Head[HeadLength - 1])
                {
                    Protocol.TargetId = buf;
                }
                else
                {
                    Index--;
                }
            }
            else if (Index == HeadLength + 1)
            {
                Protocol.SourceId = buf;
            }
            else if (Index == HeadLength + 2)
            {
                Protocol.Command = buf;
            }
            else if (Index == HeadLength + 3)
            {
                Protocol.DataLength = buf;
                if (buf > 0 && (Protocol.Data is null || buf != Protocol.Data.Length))
                {
                    Protocol.Data = new byte[buf];
                }
            }
            else if (Index < HeadToDataLength)
            {
                if (Protocol.DataLength > 0)
                {
                    Protocol.Data![Index - HeadLength - 4] = buf;
                }
            }
            else if (Index < HeadToCheckBytesLength && Protocol.HasCheckBytes)   // 仅在 Has CheckBytes 时运行
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
                    ParseNoError = true;
                    failedCallback?.Invoke(Protocol);
                }
            }
        }
    }
}
