using HqPocket.Communications;
using HqPocket.Extensions.Sockets.TcpServer;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace HqPocket.Extensions.Sockets
{
    public class TcpServerCommunicator : CommunicatorBase, ITcpServerCommunicator
    {
        private bool _isListening;

        /// <summary>
        /// key: RemoteEndPoint.ToString()
        /// </summary>
        private readonly ConcurrentDictionary<string, TcpClient> _clients = new();
        protected CancellationTokenSource? ListeningClientConnectionCts { get; set; }
        protected SocketCommunicatorOptions Options { get; }
        protected TcpListener? Server { get; set; }
        public override string Name { get; } = "TCP Server";
        public override bool IsConnected => _isListening;
        public override bool IsRemoteConnected => !_clients.IsEmpty;
        public TcpServerCommunicator(IOptions<SocketCommunicatorOptions> options)
        {
            Options = options.Value;
        }

        // Start
        public override void Connect()
        {
            CancelEventArgs e = new();
            OnConnecting(e);
            if (e.Cancel) return;

            Server = new(IPAddress.Parse(Options.HostName), Options.Port);
            Server.Start();

            _isListening = true;

            ProcessReceivedBytesTaskCts = new();
            Task.Run(() => ProcessReceivedBytes(ProcessReceivedBytesTaskCts.Token), ProcessReceivedBytesTaskCts.Token);

            ListeningClientConnectionCts = new();
            Task.Run(async () => await ListeningClientConnectionAsync(ListeningClientConnectionCts.Token), ListeningClientConnectionCts.Token);

            OnConnected(EventArgs.Empty);
        }


        private async Task ListeningClientConnectionAsync(CancellationToken token)
        {
            if (Server is null || !IsConnected) return;

            while (!token.IsCancellationRequested)
            {
                // no client connect
                TcpClient client = await Server.AcceptTcpClientAsync(token);    // wait

                // client connected
                var remoteEndPoint = client.Client.RemoteEndPoint?.ToString();
                if (remoteEndPoint is not null)
                {
                    OnRemoteConnected(new RemoteConnectionEventArgs(client.Client.LocalEndPoint?.ToString(), remoteEndPoint));
                    _ = _clients.GetOrAdd(remoteEndPoint, client);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    Task.Run(async () => await ReceiveDataAsync(client, token), token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                }
            }
            ListeningClientConnectionCts?.Dispose();
            ListeningClientConnectionCts = null;
        }

        private async Task ReceiveDataAsync(TcpClient client, CancellationToken token)
        {
            while (!token.IsCancellationRequested && client.Client?.Connected is true)
            {
                await using var stream = client.GetStream();
                int bytesToRead = client.ReceiveBufferSize;
                byte[] readBuffer = new byte[bytesToRead];

                int readCount;
                while ((readCount = await stream.ReadAsync(readBuffer.AsMemory(), token)) != 0) // wait
                {
                    // received client data
                    ReceivedBytesQueue.Enqueue(readBuffer[0..readCount]);
                }

                // client disconnected
                var remoteEntPoint = client.Client.RemoteEndPoint?.ToString();
                if (remoteEntPoint is not null)
                {
                    OnRemoteDisconnected(new RemoteConnectionEventArgs(client.Client.LocalEndPoint?.ToString(), remoteEntPoint));
                    _ = _clients.TryRemove(remoteEntPoint, out _);
                    client.Close();
                }
            }
        }

        public override void Disconnect()
        {
            CancelEventArgs e = new();
            OnDisconnecting(e);
            if (e.Cancel) return;

            _isListening = false;
            _clients.Clear();
            ListeningClientConnectionCts?.Cancel();

            if (Server is not null)
            {
                Server.Stop();
                Server = null;
            }

            ProcessReceivedBytesTaskCts?.Cancel();
            ReceivedBytesQueue.Clear();
            OnDisconnected(EventArgs.Empty);
        }

        public override void Send(IEnumerable<byte>? bytes)
        {
            if (!IsRemoteConnected) return;

            var array = bytes?.ToArray();
            if (array?.Length > 0)
            {
                foreach (var client in _clients.Values)
                {
                    client.Client.Send(array);
                }
            }
        }

        public void DisconnectClient(string clientEndPointString)
        {
            if (!_clients.TryRemove(clientEndPointString, out TcpClient? client)) return;

            client?.Client.Shutdown(SocketShutdown.Both);
            client?.Client.Disconnect(false);
            client?.Close();
        }
    }
}
