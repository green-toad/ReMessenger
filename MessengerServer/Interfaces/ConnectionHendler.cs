using NetDriver.AE;
namespace MessengerServer.Interfaces
{
    internal interface IConnectionHandler
    {
        DisconnectEvent disconnectEvent { get; }

        IncomingEvent incomingEvent { get; }
    }
}