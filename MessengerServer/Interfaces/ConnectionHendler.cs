using NetDriver.AE;
namespace MessengerServer.Interfaces
{
    internal interface IConnectionHandler : IAsyncDisposable
    {
        DisconnectEvent disconnectEvent { get; }

        IncomingEvent incomingEvent { get; }
    }
}