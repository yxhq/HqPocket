using HqPocket.Communications;
using HqPocket.Extensions.Sockets.Helpers;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net.Sockets;

namespace HqPocket.Extensions.Sockets
{
    public class UdpClientCommunicator : CommunicatorBase
    {
        private bool _isConnected;
        private bool _isRemoteConnected;
        protected CancellationTokenSource? ReceiveTaskCts { get; set; }
        protected SocketCommunicatorOptions Options { get; }
        protected UdpClient? Client { get; set; }
        public override string Name { get; } = SocketHelper.UdpClientCommunicatorName;
        public override bool IsConnected => _isConnected;   // Client?.Client.Connected ?? false;
        public override bool IsRemoteConnected => _isRemoteConnected;
        public UdpClientCommunicator(IOptions<SocketCommunicatorOptions> options)
        {
            Options = options.Value;
        }

        public override void Connect()
        {
            CancelEventArgs e = new();
            OnConnecting(e);
            if (e.Cancel) return;

            Client = new(Options.HostName, Options.Port);

            _isConnected = true;
            _isRemoteConnected = false;

            ProcessReceivedBytesTaskCts = new();
            Task.Run(() => ProcessReceivedBytes(ProcessReceivedBytesTaskCts.Token), ProcessReceivedBytesTaskCts.Token);

            ReceiveTaskCts = new();
            Task.Run(async () => await ReceiveAsync(ReceiveTaskCts.Token), ReceiveTaskCts.Token);

            OnConnected(EventArgs.Empty);
        }

        private async Task ReceiveAsync(CancellationToken token)
        {
            if (Client is null || !IsConnected) return;

            while (!token.IsCancellationRequested)
            {
                var result = await Client.ReceiveAsync(token);
                if(!_isRemoteConnected)
                {
                    _isRemoteConnected = true;
                    OnRemoteConnected(new RemoteConnectionEventArgs(Client.Client.LocalEndPoint?.ToString(), Client.Client.RemoteEndPoint?.ToString()));
                }                

                if (result.Buffer.Length > 0)
                {
                    ReceivedBytesQueue.Enqueue(result.Buffer);
                }                
            }
            ReceiveTaskCts?.Dispose();
            ReceiveTaskCts = null;
        }

        public override void Disconnect()
        {
            CancelEventArgs e = new();
            OnDisconnecting(e);
            if (e.Cancel) return;

            _isConnected = false;
            _isRemoteConnected = false;
            ReceiveTaskCts?.Cancel();

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
                Client?.Send(array);
            }
        }
    }
}
