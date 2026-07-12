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
        public void Add(Networker networker);
        public Networker GetBySocket(Socket socket);
        public bool PairWithUID(Socket socket, Guid suid);
        public Networker GetBySUID(Guid suid);
    }
    internal class ConnectionContainer : IConnectionContainer
    {
        private readonly ConcurrentDictionary<Guid?, Networker> _networkers = new();
        public void Add(Networker networker)
        {
            throw new NotImplementedException();
        }

        public Networker GetBySocket(Socket socket)
        {
            throw new NotImplementedException();
        }

        public Networker GetBySUID(Guid suid)
        {
            throw new NotImplementedException();
        }

        public bool PairWithUID(Socket socket, Guid suid)
        {
            throw new NotImplementedException();
        }
    }
}
