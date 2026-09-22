using System.Net.Sockets;
using NetDriver.AE;

namespace MessengerServer.Interfaces
{
    internal interface IConnection
    {
        Guid USUID { get; }
        Networker UNetworker { get; }
        IAsymetrycEncryptor UAsymEncryptor { get; }
        ISymetrycEncryptor USymEncryptor { get; }
        bool InitalizeNetworker(Socket socket, DisconnectEvent devent, IncomingEvent ievent);
    }
}