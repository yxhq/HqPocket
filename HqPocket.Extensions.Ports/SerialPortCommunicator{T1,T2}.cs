using HqPocket.Communications;
using Microsoft.Extensions.Options;
using System;

namespace HqPocket.Extensions.Ports;

public class SerialPortCommunicator<TProtocol, TData> : SerialPortCommunicator<TProtocol>, ICommunicator<TProtocol, TData>
    where TProtocol : IProtocol
    where TData : class
{
    public event EventHandler<TData>? DataCreated;
    public Func<TProtocol, TData>? DataCreator { get; set; }

    public SerialPortCommunicator(IOptions<SerialPortCommunicatorOptions> options) : base(options)
    {
    }

    protected override void OnReceiveParseSucceed(TProtocol protocol)
    {
        ArgumentNullException.ThrowIfNull(DataCreator);

        base.OnReceiveParseSucceed(protocol);
        var data = DataCreator(protocol);
        OnDataCreated(data);
    }

    protected virtual void OnDataCreated(TData data)
    {
        DataCreated?.Invoke(this, data);
    }
}
