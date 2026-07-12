using MessengerServer.RequestHandler;
using Microsoft.Extensions.Hosting;
using NetDriver.AE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MessengerServer.ConnectionReciver
{
    internal class ConnectionReceiver : BackgroundService
    {
        private readonly IConnectionFabric _factory;
        private ConnectionAccepter _listener;
        private IConnectionContainer _container;
        private IncomingEvent _delegateLink;

        public ConnectionReceiver(
            IConnectionFabric fabric, 
            ConnectionAccepter listener, 
            IConnectionContainer container,
            MessageHandler delegateLinkObj
            )
        {
            _factory = fabric;
            _listener = listener;
            _container = container;
            _delegateLink = delegateLinkObj.Incoming;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var sock in _listener.acceptingRequests.Reader.ReadAllAsync(stoppingToken))
            {
                _container.Add(
                    _factory.Create(
                        sock, 
                        _delegateLink
                    )
                );
            }
        }
    }
}
