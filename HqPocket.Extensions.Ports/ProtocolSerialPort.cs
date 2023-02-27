using HqPocket.Communications;
using System;
using System.ComponentModel;

namespace HqPocket.Extensions.Ports;

public class ProtocolSerialPort<TProtocol> : AsyncSerialPort, IProtocolSerialPort<TProtocol> where TProtocol : IProtocol
{
    public event EventHandler<TProtocol>? ReceiveParseSucceed;
    public event EventHandler<TProtocol>? ReceiveParseFailed;
    public IProtocolParser<TProtocol>? ReceiveProtocolParser { get; set; }

    protected override void OnBytesReceived(byte[] receivedBytes)
    {
        base.OnBytesReceived(receivedBytes);
        ReceiveProtocolParser?.Parse(receivedBytes, OnReceiveParseSucceed, OnReceiveParseFailed);
    }

    protected virtual void OnReceiveParseSucceed(TProtocol protocol)
    {
        ReceiveParseSucceed?.Invoke(this, protocol);
    }

    protected virtual void OnReceiveParseFailed(TProtocol protocol)
    {
        ReceiveParseFailed?.Invoke(this, protocol);
    }

    protected override void OnOpening(CancelEventArgs e)
    {
        base.OnOpening(e);
        ReceiveProtocolParser?.Initialize();
    }
}
