using System.Net.Sockets;
using NetDriver.AE;

namespace MessengerServer.Interfaces
{
    internal interface IConnection : IAsyncDisposable
    {
        Guid USUID { get; }
        Networker UNetworker { get; }
        IAsymetrycEncryptor UAsymEncryptor { get; }
        ISymetrycEncryptor USymEncryptor { get; }
        void InitalizeNetworker(Socket socket, DisconnectEvent devent, IncomingEvent ievent);
        bool InitalizeSUID(Guid suid);
    }
}