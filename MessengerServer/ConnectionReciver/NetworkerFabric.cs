using Microsoft.Extensions.DependencyInjection;
using NetDriver.AE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MessengerServer.ConnectionReciver
{
    public interface IConnectionFabric
    {
        public Networker Create(Socket socket, IncomingEvent incomingEvent);
    }


    public class ConnectionHandlerFactory : IConnectionFabric
    {
        private readonly IServiceProvider _serviceProvider;

        public ConnectionHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Networker Create(Socket socket, IncomingEvent incomingEvent)
        {
            return ActivatorUtilities.CreateInstance<Networker>(
                _serviceProvider, socket, incomingEvent);
        }
    }
}
