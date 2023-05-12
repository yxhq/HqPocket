namespace HqPocket.Extensions.Sockets.TcpServer
{
    public interface ITcpServerCommunicator
    {
        /// <summary>
        /// 断开已连接的某个客户端
        /// </summary>
        /// <param name="clientEndPointString">客户端EndPoint字符串</param>
        void DisconnectClient(string clientEndPointString);
    }
}
