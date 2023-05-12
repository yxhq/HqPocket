using HqPocket.Communications;
using HqPocket.Extensions.Ports.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading.Tasks;

namespace HqPocket.Extensions.Ports
{
    public class SerialPortCommunicator : CommunicatorBase
    {
        protected SerialPortCommunicatorOptions Options { get; }
        public SerialPort? SerialPort { get; private set; }
        public override string Name { get; } = SerialPortHelper.SerialPortCommunicatorName;
        public override bool IsConnected => SerialPort?.IsOpen ?? false;
        public override bool IsRemoteConnected => IsConnected;

        public SerialPortCommunicator(IOptions<SerialPortCommunicatorOptions> options)
        {
            Options = options.Value;
        }

        public override void Connect()
        {
            CancelEventArgs e = new();
            OnConnecting(e);
            if (e.Cancel) return;

            SerialPort = new(Options.PortName, Options.BaudRate, Options.Parity, Options.DataBits, Options.StopBits)
            {
                ReadBufferSize = Options.ReadBufferSize,
                WriteBufferSize = Options.WriteBufferSize
            };


            ProcessReceivedBytesTaskCts = new();
            Task.Run(() => ProcessReceivedBytes(ProcessReceivedBytesTaskCts.Token), ProcessReceivedBytesTaskCts.Token);

            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.Open();
            OnConnected(EventArgs.Empty);
        }

        public override void Disconnect()
        {
            CancelEventArgs e = new();
            OnDisconnecting(e);
            if (e.Cancel) return;

            if (SerialPort is not null)
            {
                SerialPort.DataReceived -= SerialPort_DataReceived;
                SerialPort.Close();
                SerialPort = null;
            }

            ProcessReceivedBytesTaskCts?.Cancel();
            ReceivedBytesQueue.Clear();
            OnDisconnected(EventArgs.Empty);
        }

        public override void Send(IEnumerable<byte>? bytes)
        {
            SerialPort?.Write(bytes);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is SerialPort port)
            {
                try
                {
                    int bytesToRead = port.BytesToRead;
                    byte[] readBuffer = new byte[bytesToRead];

                    if (port.Read(readBuffer, 0, bytesToRead) > 0)
                    {
                        ReceivedBytesQueue.Enqueue(readBuffer);
                    }
                }
                catch (InvalidOperationException)    // The port is closed.
                {
                    // do nothing                
                }
            }
        }
    }
}
