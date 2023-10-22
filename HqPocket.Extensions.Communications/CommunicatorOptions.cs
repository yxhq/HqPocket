using System.Threading;

namespace HqPocket.Extensions.Communications
{
    public class CommunicatorOptions
    {
        /// <summary>
        /// 是否使用更快的接收处理数据方式（接收数据时无延时处理会更快，但会占用CPU）
        /// </summary>
        public bool UseFastReceiveProcessor { get; set; } = false;

        /// <summary>
        /// 接收超时时间（ms）
        /// </summary>
        public int ReceiveTimeout { get; set; } = Timeout.Infinite;

        /// <summary>
        /// 设置超时是否从打开连接开始记，否则从发送完一帧数据后开始记
        /// </summary>
        public bool TimeoutFromConnected { get; set; } = false;
    }
}
