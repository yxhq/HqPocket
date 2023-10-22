using HqPocket.Comparers;

using System;
using System.IO.Ports;

namespace HqPocket.Extensions.Communications.Ports.Helpers;

public static class SerialPortHelper
{
    public const string SerialPortCommunicatorName = "COM";

    public static string[] GetSortedPortNames()
    {
        var serialPortNames = SerialPort.GetPortNames();
        Array.Sort(serialPortNames, new LetterNumberComparer());
        return serialPortNames;
    }

    public static int[] CreatePortBaudRates()
    {
        return new[]
        {
            110,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            14400,
            19200,
            38400,
            56000,
            57600,
            115200,
            128000,
            256000,
            614400
        };
    }

    public static int[] CreatePortDataBitses()
    {
        return new[] { 5, 6, 7, 8 };
    }
}
