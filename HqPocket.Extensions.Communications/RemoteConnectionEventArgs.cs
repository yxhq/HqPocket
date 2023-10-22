using System;

namespace HqPocket.Extensions.Communications;

public class RemoteConnectionEventArgs : EventArgs
{
    public string? LocalEndPointString { get; set; }
    public string? RemoteEndPointString { get; set; }

    public RemoteConnectionEventArgs(string? localEndPointString, string? remoteEndPointString)
    {
        LocalEndPointString = localEndPointString;
        RemoteEndPointString = remoteEndPointString;
    }
}
