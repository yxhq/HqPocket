using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets
{
    public class TcpClientCommunicatorOptions : SocketCommunicatorOptions, IOptions<TcpClientCommunicatorOptions>
    {
        TcpClientCommunicatorOptions IOptions<TcpClientCommunicatorOptions>.Value => this;
    }
}
