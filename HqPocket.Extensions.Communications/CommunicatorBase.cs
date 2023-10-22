using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace HqPocket.Extensions.Communications
{
    public abstract class CommunicatorBase : ICommunicator
    {
        private readonly CommunicatorOptions _communicatorOptions;
        protected ConcurrentQueue<byte[]> ReceivedBytesQueue { get; } = new();
        protected CancellationTokenSource? ProcessReceivedBytesTaskCts { get; set; }
        protected CancellationTokenSource? ContinuousSendTaskCts { get; set; }
        protected Timer? LoopSendTimer { get; set; }

        public event EventHandler<byte[]>? BytesReceived;
        public event EventHandler<CancelEventArgs>? Connecting;
        public event EventHandler<EventArgs>? Connected;
        public event EventHandler<CancelEventArgs>? Disconnecting;
        public event EventHandler<EventArgs>? Disconnected;
        public event EventHandler<RemoteConnectionEventArgs>? RemoteConnected;
        public event EventHandler<RemoteConnectionEventArgs>? RemoteDisconnected;
        public abstract string Name { get; }
        public abstract bool IsConnected { get; }
        public abstract bool IsRemoteConnected { get; }

        protected CommunicatorBase(CommunicatorOptions communicatorOptions)
        {
            _communicatorOptions = communicatorOptions;
        }

        public abstract void Connect();
        public abstract void Disconnect();
        public abstract void Send(IEnumerable<byte>? bytes);

        public virtual void LoopSend(IEnumerable<byte>? bytes, TimeSpan period, Action? loopCallback = null)
        {
            if (period == TimeSpan.Zero)
            {
                ContinuousSendTaskCts = new();
                Task.Run(() =>
                {
                    while (!ContinuousSendTaskCts.IsCancellationRequested)
                    {
                        Send(bytes);
                        loopCallback?.Invoke();
                    }
                    ContinuousSendTaskCts.Dispose();
                    ContinuousSendTaskCts = null;

                }, ContinuousSendTaskCts.Token);
            }
            else
            {
                LoopSendTimer = new Timer(_ =>
                {
                    Send(bytes);
                    loopCallback?.Invoke();
                }, null, TimeSpan.Zero, period);
            }
        }


        public virtual void StopLoopSend()
        {
            ContinuousSendTaskCts?.Cancel();

            if (LoopSendTimer is not null)
            {
                LoopSendTimer.Change(Timeout.Infinite, Timeout.Infinite);
                LoopSendTimer.Dispose();
                LoopSendTimer = null;
            }
        }

        protected virtual void ProcessReceivedBytes(CancellationToken token)
        {
            if (_communicatorOptions.UseFastReceiveProcessor)
            {
                while (!token.IsCancellationRequested)
                {
                    if (ReceivedBytesQueue.TryDequeue(out var receiveBytesBuffer))
                    {
                        OnBytesReceived(receiveBytesBuffer);
                    }
                }
            }
            else
            {
                while (!token.IsCancellationRequested)
                {
                    if (ReceivedBytesQueue.IsEmpty)
                    {
                        try
                        {
                            Task.Delay(1, token).Wait(token);
                        }
                        catch (OperationCanceledException)
                        {
                        }
                    }
                    else
                    {
                        while (ReceivedBytesQueue.TryDequeue(out var receiveBytesBuffer))
                        {
                            OnBytesReceived(receiveBytesBuffer);
                        }
                    }
                }
            }
            ProcessReceivedBytesTaskCts?.Dispose();
            ProcessReceivedBytesTaskCts = null;
        }


        protected virtual void OnBytesReceived(byte[] receivedBytes)
        {
            BytesReceived?.Invoke(this, receivedBytes);
        }

        protected virtual void OnConnecting(CancelEventArgs e)
        {
            Connecting?.Invoke(this, e);
        }

        protected virtual void OnConnected(EventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        protected virtual void OnDisconnecting(CancelEventArgs e)
        {
            Disconnecting?.Invoke(this, e);
        }

        protected virtual void OnDisconnected(EventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        protected virtual void OnRemoteConnected(RemoteConnectionEventArgs e)
        {
            RemoteConnected?.Invoke(this, e);
        }

        protected virtual void OnRemoteDisconnected(RemoteConnectionEventArgs e)
        {
            RemoteDisconnected?.Invoke(this, e);
        }
    }
}
