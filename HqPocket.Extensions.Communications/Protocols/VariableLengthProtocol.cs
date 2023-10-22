using System;
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.Extensions.Communications;

/// <summary>
/// 通讯协议： 帧头 + 数据长度(1字节) + 数据 + [校验] + [尾帧]
/// 避免帧头与数据长度字节相同
/// 帧头：长度固定，帧头内容可以不同
/// 校验：数据长度、数据
/// </summary>
public class VariableLengthProtocol : ProtocolBase
{
    /// <summary>
    /// 数据长度
    /// </summary>
    public byte DataLength { get; set; }

    public VariableLengthProtocol(byte[] head, ICheckMethod? checkMethod = null, byte[]? tail = null)
        : base(head, checkMethod, tail)
    {
    }

    public VariableLengthProtocol(byte headByte, int headLength, ICheckMethod? checkMethod = null, byte[]? tail = null)
        : this(new byte[headLength], checkMethod, tail)
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

        DataLength = (byte)Data.Length;
        yield return DataLength;

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
        IEnumerable<byte> willBeCheckBytes = new byte[] { DataLength };
        if (DataLength > 0)
        {
            willBeCheckBytes = willBeCheckBytes.Concat(Data!);
        }
        return CheckMethod!.Check(willBeCheckBytes);
    }
}
