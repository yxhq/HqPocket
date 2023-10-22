using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets;

public class TcpServerCommunicator<TProtocol> : TcpServerCommunicator, ICommunicator<TProtocol> where TProtocol : IProtocol
{
    private readonly object _timerLocker = new();
    private Timer? _timeoutTimer;

    public event EventHandler<TProtocol>? ReceiveParseSucceed;
    public event EventHandler<TProtocol>? ReceiveParseFailed;
    public event EventHandler<TProtocol>? ReceiveTimedOut;
    public IProtocolParser<TProtocol>? ReceiveProtocolParser { get; set; }

    public TcpServerCommunicator(IOptions<TcpServerCommunicatorOptions> options) : base(options)
    {
    }

    protected override void OnBytesReceived(byte[] receivedBytes)
    {
        base.OnBytesReceived(receivedBytes);
        ReceiveProtocolParser?.Parse(receivedBytes, OnReceiveParseSucceed, OnReceiveParseFailed);
    }

    protected virtual void OnReceiveParseSucceed(TProtocol protocol)
    {
        lock (_timerLocker)
        {
            _timeoutTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        ReceiveParseSucceed?.Invoke(this, protocol);
    }

    protected virtual void OnReceiveParseFailed(TProtocol protocol)
    {
        lock (_timerLocker)
        {
            _timeoutTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        ReceiveParseFailed?.Invoke(this, protocol);
    }

    protected virtual void OnReceiveTimedOut(TProtocol protocol)
    {
        ReceiveTimedOut?.Invoke(this, protocol);
    }

    protected override void OnConnected(EventArgs e)
    {
        base.OnConnected(e);
        if (ReceiveProtocolParser is null) return;

        ReceiveProtocolParser.Initialize();
        lock (_timerLocker)
        {
            int timeout = Options.TimeoutFromConnected ? Options.ReceiveTimeout : Timeout.Infinite;
            _timeoutTimer = new Timer(_ => OnReceiveTimedOut(ReceiveProtocolParser.Protocol), null, timeout, Timeout.Infinite);
        }
    }

    public override void Send(IEnumerable<byte>? bytes)
    {
        base.Send(bytes);

        lock (_timerLocker)
        {
            _timeoutTimer?.Change(Options.ReceiveTimeout, Timeout.Infinite);
        }
    }

    protected override void OnDisconnected(EventArgs e)
    {
        lock (_timerLocker)
        {
            if (_timeoutTimer is not null)
            {
                _timeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _timeoutTimer.Dispose();
                _timeoutTimer = null;
            }
        }

        base.OnDisconnected(e);
    }
}
