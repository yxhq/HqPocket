using System;
using System.Collections.Generic;
using System.Linq;

namespace HqPocket.Communications;

public abstract class ProtocolBase : IProtocol
{
    /// <summary>
    /// 帧头
    /// </summary>
    public byte[] Head { get; set; }

    /// <summary>
    /// 数据（不包括帧头，校验位）
    /// </summary>
    public byte[]? Data { get; set; }

    /// <summary>
    /// 校验
    /// </summary>
    public byte[]? CheckBytes { get; set; }

    /// <summary>
    /// 帧尾（如果有）,有帧尾时必须具有校验字节
    /// </summary>
    public byte[]? Tail { get; set; }

    /// <summary>
    /// 校验方法（如果有校验）
    /// </summary>
    public ICheckMethod? CheckMethod { get; set; }

    public bool HasCheckBytes { get; protected set; }
    public bool HasTail { get; protected set; }

    protected ProtocolBase(byte[] head, ICheckMethod? checkMethod, byte[]? tail)
    {
        Head = head;
        CheckMethod = checkMethod;

        if (CheckMethod is not null)
        {
            CheckBytes = new byte[CheckMethod.CheckByteCount];
            HasCheckBytes = true;
        }

        if (tail is not null && tail.Length > 0)
        {
            Tail = tail;
            HasTail = true;
        }
    }

    public virtual bool IsCheckedCorrect()
    {
        ArgumentNullException.ThrowIfNull(CheckMethod);
        ArgumentNullException.ThrowIfNull(CheckBytes);

        return Enumerable.SequenceEqual(CreateCheckBytes(), CheckBytes);
    }

    public abstract IEnumerable<byte> CreateProtocolSequence();

    protected abstract IEnumerable<byte> CreateCheckBytes();
}
