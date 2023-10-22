using Microsoft.Extensions.Options;
using System.IO.Ports;

namespace HqPocket.Extensions.Communications.Ports
{
    public class SerialPortCommunicatorOptions : CommunicatorOptions, IOptions<SerialPortCommunicatorOptions>
    {
        public int DataBits { get; set; } = 8;
        public Handshake Handshake { get; set; } = Handshake.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Parity Parity { get; set; } = Parity.None;
        public int BaudRate { get; set; } = 9600;
        public int ReadBufferSize { get; set; } = 409600;
        public int WriteBufferSize { get; set; } = 2048;
        public string PortName { get; set; } = "COM1";

        SerialPortCommunicatorOptions IOptions<SerialPortCommunicatorOptions>.Value => this;
    }
}
