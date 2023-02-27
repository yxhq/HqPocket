using HqPocket.Communications;
using System;

namespace HqPocket.Extensions.Ports;

/// <summary>
/// 按一定通讯协议读取、写入串口数据，并创建相关数据。
/// 执行顺序：BytesReceived -> ReceiveProtocolParser -> ReceiveParseSucceed -> DataCreator -> DataCreated
/// </summary>
/// <typeparam name="TProtocol">设定的通讯协议</typeparam>
/// <typeparam name="TData">要创建的数据类型</typeparam>
public interface IProtocolDataSerialPort<TProtocol, TData> : IProtocolSerialPort<TProtocol> where TProtocol : IProtocol
{
    /// <summary>
    /// 数据创建成功后
    /// </summary>
    event EventHandler<TData>? DataCreated;
    /// <summary>
    /// 创建数据所使用的委托
    /// </summary>
    Func<TProtocol, TData>? DataCreator { get; set; }
}