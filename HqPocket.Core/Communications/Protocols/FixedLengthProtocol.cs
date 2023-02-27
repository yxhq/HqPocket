using System;
using System.Collections.Generic;

namespace HqPocket.Communications;

/// <summary>
/// 通讯协议： 帧头 + 数据 + [校验] + [尾帧]
/// 只判断帧头，不判断帧头后的第一位数据
/// 帧头：长度固定，帧头内容可以不同
/// 校验：数据
/// </summary>
public class FixedLengthProtocol : ProtocolBase
{
    public FixedLengthProtocol(byte[] head, int dataLength, ICheckMethod? checkMethod = null, byte[]? tail = null)
        : base(head, checkMethod, tail)
    {
        Data = new byte[dataLength];
    }

    public FixedLengthProtocol(byte headByte, int headLength, int dataLength, ICheckMethod? checkMethod = null, byte[]? tail = null)
        : this(new byte[headLength], dataLength, checkMethod, tail)
    {
        Array.Fill(Head, headByte);
    }

    public override IEnumerable<byte> CreateProtocolSequence()
    {
        ArgumentNullException.ThrowIfNull(Data);

        foreach (var head in Head)
        {
            yield return head;
        }

        foreach (var data in Data)
        {
            yield return data;
        }

        if (HasCheckBytes)
        {
            foreach (var checkByte in CreateCheckBytes())
            {
                yield return checkByte;
            }
        }

        if (HasTail)
        {
            foreach (var tail in Tail!)
            {
                yield return tail;
            }
        }
    }

    protected override IEnumerable<byte> CreateCheckBytes()
    {
        return CheckMethod!.Check(Data!);
    }
}
