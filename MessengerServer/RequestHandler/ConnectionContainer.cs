using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using NetDriver.AE;

namespace MessengerServer.RequestHandler
{
    public interface IHashContainer<T>
    {
        public bool Add(Socket socket);
        public bool Pop(Socket socket);
        public bool Pop(T content);
        public T? Get(Socket socket);

        public List<T> GetAll();
    }
    internal class ConnectionContainer : IHashContainer<ClientInformation>
    {
        private readonly ConcurrentDictionary<Socket, ClientInformation> _clientInfo = new();
        public bool Add(Socket socket)
        {
            return _clientInfo.TryAdd(socket, new ClientInformation());
        }

        public ClientInformation Get(Socket socket)
        {
            if (_clientInfo.TryGetValue(socket, out var ci)) return ci;
            return new ClientInformation();         // здесь должен быть null но, почему то интерфейс не хочет его кушать. . . позже поменяю.
        }

        public List<ClientInformation> GetAll()
        {
            return _clientInfo.Values.ToList();
        }

        public bool Pop(Socket socket)
        {
            return _clientInfo.TryRemove(socket, out _);
        }

        public bool Pop(ClientInformation content)
        {
            return _clientInfo.TryRemove(_clientInfo.First(c => c.Value.Equals(content)).Key, out _);
        }
    }

    public struct ClientInformation
    {
        public Networker networker;
        public UInt64 suid;
        public UInt16 loginAttemptsNumber;              // пока не нужен, но не забудь вставить в регистрацию и аутентификацию  . . .
    }
}
