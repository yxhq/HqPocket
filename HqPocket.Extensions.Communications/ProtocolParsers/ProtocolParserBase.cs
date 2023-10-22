using System;

namespace HqPocket.Extensions.Communications;

public abstract class ProtocolParserBase<TProtocol> : IProtocolParser<TProtocol> where TProtocol : IProtocol
{
    /// <summary>
    /// 解析数据时的索引
    /// </summary>
    protected virtual int Index { get; set; } = -1;

    /// <summary>
    /// 解析过程数据无错误
    /// </summary>
    protected virtual bool ParseNoError { get; set; } = true;

    /// <summary>
    /// 帧头长度
    /// </summary>
    protected virtual int HeadLength => Protocol.Head.Length;

    /// <summary>
    /// 帧头+[命令字]+数据 长度
    /// </summary>
    protected abstract int HeadToDataLength { get; }

    /// <summary>
    /// 帧头+[命令字]+数据+[校验] 长度
    /// </summary>
    protected virtual int HeadToCheckBytesLength => HeadToDataLength + (Protocol.CheckMethod?.CheckByteCount ?? 0);

    /// <summary>
    /// 帧头+[命令字]+数据+[校验]+[尾帧] 长度
    /// </summary>
    protected virtual int HeadToTailLength => HeadToCheckBytesLength + (Protocol.Tail?.Length ?? 0);

    public TProtocol Protocol { get; set; }

    protected ProtocolParserBase(TProtocol protocol)
    {
        Protocol = protocol;
    }

    public virtual void Initialize()
    {
        Index = -1;
        ParseNoError = true;
    }

    public abstract void Parse(ReadOnlySpan<byte> bytes, Action<TProtocol>? succeedCallback, Action<TProtocol>? failedCallback);
}
