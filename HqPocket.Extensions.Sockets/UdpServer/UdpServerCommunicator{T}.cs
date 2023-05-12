using HqPocket.Communications;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace HqPocket.Extensions.Sockets;

public class UdpServerCommunicator<TProtocol> : UdpServerCommunicator, ICommunicator<TProtocol> where TProtocol : IProtocol
{
    private Timer? _timeoutTimer;

    public event EventHandler<TProtocol>? ReceiveParseSucceed;
    public event EventHandler<TProtocol>? ReceiveParseFailed;
    public event EventHandler? ReceiveTimedOut;
    public IProtocolParser<TProtocol>? ReceiveProtocolParser { get; set; }
    public int TimeoutPeriod { get; set; } = Timeout.Infinite;

    public UdpServerCommunicator(IOptions<SocketCommunicatorOptions> options) : base(options)
    {
    }

    protected override void OnBytesReceived(byte[] receivedBytes)
    {
        base.OnBytesReceived(receivedBytes);
        if (ReceiveProtocolParser is null)
        {
            _timeoutTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }
        ReceiveProtocolParser?.Parse(receivedBytes, OnReceiveParseSucceed, OnReceiveParseFailed);
    }

    protected virtual void OnReceiveParseSucceed(TProtocol protocol)
    {
        _timeoutTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        ReceiveParseSucceed?.Invoke(this, protocol);
    }

    protected virtual void OnReceiveParseFailed(TProtocol protocol)
    {
        _timeoutTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        ReceiveParseFailed?.Invoke(this, protocol);
    }

    protected virtual void OnReceiveTimedOut(EventArgs e)
    {
        ReceiveTimedOut?.Invoke(this, e);
    }

    protected override void OnConnecting(CancelEventArgs e)
    {
        base.OnConnecting(e);

        ReceiveProtocolParser?.Initialize();
        _timeoutTimer = new Timer(_ => OnReceiveTimedOut(EventArgs.Empty), null, Timeout.Infinite, Timeout.Infinite);
    }

    public override void Send(IEnumerable<byte>? bytes)
    {
        base.Send(bytes);
        _timeoutTimer?.Change(TimeoutPeriod, Timeout.Infinite);
    }

    protected override void OnDisconnected(EventArgs e)
    {
        if (_timeoutTimer is not null)
        {
            _timeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _timeoutTimer.Dispose();
            _timeoutTimer = null;
        }

        base.OnDisconnected(e);
    }
}
