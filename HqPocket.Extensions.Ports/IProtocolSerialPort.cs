using HqPocket.Communications;
using System;

namespace HqPocket.Extensions.Ports;

/// <summary>
/// 按一定通讯协议读取、写入串口数据
/// 执行顺序：BytesReceived -> ReceiveProtocolParser -> ReceiveParseSucceed / ReceiveParseFailed
/// </summary>
/// <typeparam name="TProtocol">设定的通讯协议</typeparam>
public interface IProtocolSerialPort<TProtocol> : IAsyncSerialPort where TProtocol : IProtocol
{
    /// <summary>
    /// 按照所设定的通讯协议接收一帧数据解析成功时
    /// </summary>
    event EventHandler<TProtocol>? ReceiveParseSucceed;
    /// <summary>
    /// 按照所设定的通讯协议接收一帧数据解析失败（校验出错）时
    /// </summary>
    event EventHandler<TProtocol>? ReceiveParseFailed;
    /// <summary>
    /// 所设定协议的解析器，即串口读取到数据后，根据所设定的协议通过该接口判断是否满足所设置的通讯协议。        
    /// </summary>
    IProtocolParser<TProtocol>? ReceiveProtocolParser { get; set; }        
}