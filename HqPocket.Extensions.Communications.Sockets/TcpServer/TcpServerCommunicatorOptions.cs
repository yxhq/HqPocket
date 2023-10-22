using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets
{
    public class TcpServerCommunicatorOptions : SocketCommunicatorOptions, IOptions<TcpServerCommunicatorOptions>
    {
        TcpServerCommunicatorOptions IOptions<TcpServerCommunicatorOptions>.Value => this;
    }
}
