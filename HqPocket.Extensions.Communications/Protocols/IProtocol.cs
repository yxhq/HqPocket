using System.Collections.Generic;

namespace HqPocket.Extensions.Communications;


/// <summary>
/// 通讯协议
/// </summary>
public interface IProtocol
{
    /// <summary>
    /// 帧头
    /// </summary>
    byte[] Head { get; set; }

    /// <summary>
    /// 数据（不包括帧头，校验位）
    /// </summary>
    byte[]? Data { get; set; }

    /// <summary>
    /// 校验
    /// </summary>
    byte[]? CheckBytes { get; set; }

    /// <summary>
    /// 尾帧
    /// </summary>
    public byte[]? Tail { get; set; }

    /// <summary>
    /// 校验方法（如果有校验）
    /// </summary>
    ICheckMethod? CheckMethod { get; set; }

    bool HasCheckBytes { get; }
    bool HasTail { get; }

    bool IsCheckedCorrect();
    IEnumerable<byte> CreateProtocolSequence();
}
