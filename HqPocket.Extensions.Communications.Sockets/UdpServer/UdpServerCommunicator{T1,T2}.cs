using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets;

public class UdpServerCommunicator<TProtocol, TData> : UdpServerCommunicator<TProtocol>, ICommunicator<TProtocol, TData>
    where TProtocol : IProtocol
    where TData : class
{
    public event EventHandler<TData>? DataCreated;
    public Func<TProtocol, TData>? DataCreator { get; set; }

    public UdpServerCommunicator(IOptions<UdpServerCommunicatorOptions> options) : base(options)
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
