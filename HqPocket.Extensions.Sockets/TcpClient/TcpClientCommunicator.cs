using HqPocket.Communications;
using HqPocket.Extensions.Sockets.Helpers;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net.Sockets;

namespace HqPocket.Extensions.Sockets
{
    public class TcpClientCommunicator : CommunicatorBase
    {
        protected CancellationTokenSource? ReceiveTaskCts { get; set; }
        protected SocketCommunicatorOptions Options { get; }
        protected TcpClient? Client { get; set; }
        protected NetworkStream? Stream { get; set; }
        public override string Name { get; } = SocketHelper.TcpClientCommunicatorName;
        public override bool IsConnected => Client?.Client?.Connected ?? false;
        public override bool IsRemoteConnected => IsConnected;
        public TcpClientCommunicator(IOptions<SocketCommunicatorOptions> options)
        {
            Options = options.Value;
        }

        public override void Connect()
        {
            CancelEventArgs e = new();
            OnConnecting(e);
            if (e.Cancel) return;

            Client = new(Options.HostName, Options.Port)
            {
                ReceiveBufferSize = Options.ReceiveBufferSize,
                SendBufferSize = Options.SendBufferSize
            };

            ProcessReceivedBytesTaskCts = new();
            Task.Run(() => ProcessReceivedBytes(ProcessReceivedBytesTaskCts.Token), ProcessReceivedBytesTaskCts.Token);

            ReceiveTaskCts = new();
            Task.Run(async () => await ReceiveAsync(ReceiveTaskCts.Token), ReceiveTaskCts.Token);

            OnConnected(EventArgs.Empty);
            OnRemoteConnected(new RemoteConnectionEventArgs(Client.Client.LocalEndPoint?.ToString(), Client.Client.RemoteEndPoint?.ToString()));
        }

        private async Task ReceiveAsync(CancellationToken token)
        {
            if (Client is null || !IsConnected) return;

            Stream = Client.GetStream();
            while (!token.IsCancellationRequested && IsConnected)
            {
                int bytesToRead = Client.ReceiveBufferSize;
                byte[] readBuffer = new byte[bytesToRead];

                int readCount;
                while ((readCount = await Stream.ReadAsync(readBuffer.AsMemory(), token)) != 0)
                {
                    // received server data
                    ReceivedBytesQueue.Enqueue(readBuffer[0..readCount]);
                }
                // server closed
                OnRemoteDisconnected(new RemoteConnectionEventArgs(Client.Client.LocalEndPoint?.ToString(), Client.Client.RemoteEndPoint?.ToString()));
                Disconnect();
            }
            ReceiveTaskCts?.Dispose();
            ReceiveTaskCts = null;
        }

        public override void Disconnect()
        {
            CancelEventArgs e = new();
            OnDisconnecting(e);
            if (e.Cancel) return;

            ReceiveTaskCts?.Cancel();

            if (Stream is not null)
            {
                Stream.Close();
                Stream = null;
            }

            if (Client is not null)
            {
                Client.Close();
                Client = null;
            }

            ProcessReceivedBytesTaskCts?.Cancel();
            ReceivedBytesQueue.Clear();
            OnDisconnected(EventArgs.Empty);
        }

        public override void Send(IEnumerable<byte>? bytes)
        {
            var array = bytes?.ToArray();
            if (array?.Length > 0)
            {
                Stream?.Write(array);
            }
        }
    }
}
