using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets
{
    public class UdpServerCommunicatorOptions : SocketCommunicatorOptions, IOptions<UdpServerCommunicatorOptions>
    {
        UdpServerCommunicatorOptions IOptions<UdpServerCommunicatorOptions>.Value => this;
    }
}
