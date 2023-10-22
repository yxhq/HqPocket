using Microsoft.Extensions.Options;

namespace HqPocket.Extensions.Communications.Sockets;

public class TcpServerCommunicator<TProtocol, TData> : TcpServerCommunicator<TProtocol>, ICommunicator<TProtocol, TData>
    where TProtocol : IProtocol
    where TData : class
{
    public event EventHandler<TData>? DataCreated;
    public Func<TProtocol, TData>? DataCreator { get; set; }

    public TcpServerCommunicator(IOptions<TcpServerCommunicatorOptions> options) : base(options)
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
