using System;
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.Extensions.Communications;

/// <summary>
/// 通讯协议： 帧头 + 目标地址(1字节) + 源地址(1字节) + 命令字(1字节) + 数据长度(1字节) + 数据 + [校验] + [尾帧]
/// 避免帧头与目标地址字节相同
/// 帧头：长度固定，帧头内容可以不同
/// 校验：目标地址、源地址、命令字、数据长度、数据
/// </summary>
public class FlexibleProtocol : ProtocolBase
{
    /// <summary>
    /// 目标地址
    /// </summary>
    public byte TargetId { get; set; }

    /// <summary>
    /// 源地址
    /// </summary>
    public byte SourceId { get; set; }

    /// <summary>
    /// 命令字
    /// </summary>
    public byte Command { get; set; }

    /// <summary>
    /// 数据长度
    /// </summary>
    public byte DataLength { get; set; }

    public FlexibleProtocol(byte[] head, ICheckMethod? checkMethod = null, byte[]? tail = null)
         : base(head, checkMethod, tail)
    {
    }

    public FlexibleProtocol(byte headByte, int headLength, ICheckMethod? checkMethod = null, byte[]? tail = null)
        : this(new byte[headLength], checkMethod, tail)
    {
        Array.Fill(Head, headByte);
    }

    public override IEnumerable<byte> CreateProtocolSequence()
    {
        foreach (var head in Head)
        {
            yield return head;
        }

        yield return TargetId;
        yield return SourceId;
        yield return Command;
        DataLength = (byte)(Data?.Length ?? 0);
        yield return DataLength;

        if (DataLength > 0)
        {
            foreach (var data in Data!)
            {
                yield return data;
            }
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
        IEnumerable<byte> willBeCheckBytes = new byte[] { TargetId, SourceId, Command, DataLength };        
        if (DataLength > 0)
        {
            willBeCheckBytes = willBeCheckBytes.Concat(Data!);
        }
        return CheckMethod!.Check(willBeCheckBytes);
    }
}
