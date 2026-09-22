using System.Net.Sockets;

namespace MessengerServer.Interfaces
{
    internal interface IConnectionContainer
    {
        Guid GetUserSUID(Socket socket);
        bool AddNewConnection(Socket socket, Guid suid);
        bool AddLazyConnection(Socket socket);
        bool CompliteLazyConnection(Socket socket, Guid suid);
    }
}