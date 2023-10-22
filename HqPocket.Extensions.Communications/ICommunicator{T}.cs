using System;

namespace HqPocket.Extensions.Communications
{
    public interface ICommunicator<TProtocol> : ICommunicator where TProtocol : IProtocol
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
        /// 接收超时
        /// </summary>
        event EventHandler<TProtocol>? ReceiveTimedOut;
        /// <summary>
        /// 所设定协议的解析器，根据所设定的协议通过该接口判断是否满足所设置的通讯协议。        
        /// </summary>
        IProtocolParser<TProtocol>? ReceiveProtocolParser { get; set; }
    }
}
