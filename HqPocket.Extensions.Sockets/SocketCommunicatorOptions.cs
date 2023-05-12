using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Sockets;

public class SocketCommunicatorOptions : IOptions<SocketCommunicatorOptions>
{
    public string HostName { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 8080;
    public int ReceiveBufferSize { get; set; } = 8192;
    public int SendBufferSize { get; set; } = 8192;
    public int Timeout { get; set; } = -1;
    SocketCommunicatorOptions IOptions<SocketCommunicatorOptions>.Value => this;
}
