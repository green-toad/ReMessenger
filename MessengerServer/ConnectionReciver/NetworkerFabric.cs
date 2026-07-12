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


    public class ConnectionHandlerFactory : IConnectionFabric // сделать так, что бы Create возвращал бы Task<Connection> и эту штуку отдельно бы обробавтывала сторонняя таска или, например, та же самая. . . типо Task.WhenAny() а внутри проводилось бы соеденение и подтягивание данных от пользователя. . . что то вроде мини хендлера, но только на подключения. . .
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
