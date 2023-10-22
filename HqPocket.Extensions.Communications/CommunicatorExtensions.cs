namespace HqPocket.Extensions.Communications;

public static class CommunicatorExtensions
{
    public static void Send(this ICommunicator communicator, IProtocol protocol)
    {
        communicator.Send(protocol.CreateProtocolSequence());
    }
}
