using System.IO.Ports;

namespace HqPocket.Extensions.Communications.Ports
{
    public interface ISerialPortCommunicator : ICommunicator
    {
        SerialPort? SerialPort { get; }
    }
}