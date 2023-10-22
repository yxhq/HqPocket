using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets
{
    public class UdpClientCommunicatorOptions : SocketCommunicatorOptions, IOptions<UdpClientCommunicatorOptions>
    {
        UdpClientCommunicatorOptions IOptions<UdpClientCommunicatorOptions>.Value => this;
    }
}
