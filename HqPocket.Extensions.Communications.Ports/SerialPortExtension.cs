using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
// ReSharper disable CheckNamespace

namespace HqPocket.Extensions.Communications.Ports;

public static class SerialPortExtension
{
    public static void Write(this SerialPort serialPort, byte[] sequence)
    {
        serialPort.Write(sequence, 0, sequence.Length);
    }

    public static void Write(this SerialPort serialPort, IEnumerable<byte>? sequence)
    {
        var bytes = sequence?.ToArray();
        if (bytes?.Length > 0)
        {
            serialPort.Write(bytes);
        }
    }
}
