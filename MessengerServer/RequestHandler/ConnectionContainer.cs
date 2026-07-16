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
        public bool Pop(Socket socket);
        public bool Pop(Networker networker);
        public Networker? GetNetworker(Socket socket);
        public bool AddSuid(Socket socket, UInt64 suid);
        public UInt64? GetSuid(Socket socket);

        public List<Networker> GetAllWorkers();
        public void Sync();
    }
    internal class ConnectionContainer : IConnectionContainer
    {
        private readonly ConcurrentDictionary<Socket, Networker> _networkers = new();
        private readonly ConcurrentDictionary<Socket, UInt64> _guids = new();

        public bool AddNetworker(Socket socket, Networker networker)
        {
            return _networkers.TryAdd(socket, networker);
        }

        public bool AddSuid(Socket socket, UInt64 suid)
        {
            return _guids.TryAdd(socket, suid);
        }

        public List<Networker> GetAllWorkers()
        {
            return _networkers.Values.ToList();
        }

        public Networker? GetNetworker(Socket socket)
        {
            if (_networkers.TryGetValue(socket, out var networker))
                return networker;
            return null;
        }

        public UInt64? GetSuid(Socket socket)
        {
            if (_guids.TryGetValue(socket, out var suid))
                return suid;
            return null;
        }

        public bool Pop(Socket socket)
        {
            if (!_networkers.TryRemove(socket, out _)) return false;
            if (!_guids.TryRemove(socket, out _)) return false;
            return true;
        }

        public bool Pop(Networker networker)
        {
            var sock = _networkers.FirstOrDefault(p => p.Value == networker).Key;
            if (sock == null) return false;

            if (!_networkers.TryRemove(sock, out _)) return false;
            if (!_guids.TryRemove(sock, out _)) return false;
            return true;
        }

        public void Sync()
        {
            foreach (var sock in _guids.Keys)
            {
                if (!_networkers.ContainsKey(sock))
                {
                    _guids.Remove(sock, out _);
                }
            }
        }
    }
}
