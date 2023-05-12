using HqPocket.Communications;
using HqPocket.Extensions.Sockets.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace HqPocket.Extensions.Sockets
{
    public class UdpServerCommunicator : CommunicatorBase
    {
        private bool _isRunning;
        private readonly ConcurrentDictionary<string, SocketReceiveFromResult> _socketReceiveFromResults = new();
        protected CancellationTokenSource? ReceiveTaskCts { get; set; }
        protected SocketCommunicatorOptions Options { get; }
        protected Socket? Server { get; set; }
        public override string Name { get; } = SocketHelper.UdpServerCommunicatorName;
        public override bool IsConnected => _isRunning;
        public override bool IsRemoteConnected => !_socketReceiveFromResults.IsEmpty;
        public UdpServerCommunicator(IOptions<SocketCommunicatorOptions> options)
        {
            Options = options.Value;
        }

        public override void Connect()
        {
            CancelEventArgs e = new();
            OnConnecting(e);
            if (e.Cancel) return;

            Server = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint local = new(IPAddress.Parse(Options.HostName), Options.Port);
            Server.Bind(local);

            _isRunning = true;

            ProcessReceivedBytesTaskCts = new();
            Task.Run(() => ProcessReceivedBytes(ProcessReceivedBytesTaskCts.Token), ProcessReceivedBytesTaskCts.Token);

            ReceiveTaskCts = new();
            Task.Run(async () => await ReceiveAsync(ReceiveTaskCts.Token), ReceiveTaskCts.Token);

            OnConnected(EventArgs.Empty);
        }

        private async Task ReceiveAsync(CancellationToken token)
        {
            if (Server is null || !IsConnected) return;

            IPEndPoint remote = new(IPAddress.Any, 0);

            while (!token.IsCancellationRequested)
            {
                byte[] receivedBytes = new byte[Options.ReceiveBufferSize];

                var receiveFromResult = await Server.ReceiveFromAsync(receivedBytes, SocketFlags.None, remote, token);  // wait
                var remoteEndPoint = receiveFromResult.RemoteEndPoint?.ToString();
                if (remoteEndPoint is not null)
                {
                    if (_socketReceiveFromResults.TryAdd(remoteEndPoint, receiveFromResult))
                    {
                        OnRemoteConnected(new RemoteConnectionEventArgs(Server.LocalEndPoint?.ToString(), remoteEndPoint));
                    }

                    if (receiveFromResult.ReceivedBytes > 0)
                    {
                        ReceivedBytesQueue.Enqueue(receivedBytes[0..receiveFromResult.ReceivedBytes]);
                    }
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

            _isRunning = false;
            _socketReceiveFromResults.Clear();
            ReceiveTaskCts?.Cancel();

            if (Server is not null)
            {
                Server.Close();
                Server = null;
            }

            ProcessReceivedBytesTaskCts?.Cancel();
            ReceivedBytesQueue.Clear();
            OnDisconnected(EventArgs.Empty);
        }

        public override void Send(IEnumerable<byte>? bytes)
        {
            if (Server is null || bytes is null || !bytes.Any()) return;

            var array = bytes.ToArray();
            foreach (var endPoint in _socketReceiveFromResults.Values.Where(r => r.RemoteEndPoint is not null).Select(r => r.RemoteEndPoint))
            {
                Server.SendTo(array, endPoint);
            }
        }
    }
}
