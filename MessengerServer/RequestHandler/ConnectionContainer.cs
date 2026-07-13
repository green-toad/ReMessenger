using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using NetDriver.AE;

namespace MessengerServer.RequestHandler
{
    public interface IConnectionContainer
    {
        public bool AddNetworker(Socket socket, Networker networker);
        public Networker? GetNetworker(Socket socket);
        public bool AddSuid(Socket socket, Guid suid);
        public Guid? GetSuid(Socket socket);
    }
    internal class ConnectionContainer : IConnectionContainer
    {
        private readonly ConcurrentDictionary<Socket, Networker> _networkers = new();
        private readonly ConcurrentDictionary<Socket, Guid> _guids = new();

        public bool AddNetworker(Socket socket, Networker networker)
        {
            return _networkers.TryAdd(socket, networker);
        }

        public bool AddSuid(Socket socket, Guid suid)
        {
            return _guids.TryAdd(socket, suid);
        }

        public Networker? GetNetworker(Socket socket)
        {
            if (_networkers.TryGetValue(socket, out var networker))
                return networker;
            return null;
        }

        public Guid? GetSuid(Socket socket)
        {
            if (_guids.TryGetValue(socket, out var suid))
                return suid;
            return null;
        }
    }
}
