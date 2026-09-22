using System.Collections.Concurrent;
using System.Net.Sockets;
using MessengerServer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessengerServer.Serices
{
    internal class ConnectionAccepter : BackgroundService
    {
        private readonly Socket _listener;
        private readonly ConcurrentDictionary<Socket, IConnection> _conContainer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectionHandler _connectionHandler;
        public ConnectionAccepter(
            ConcurrentDictionary<Socket, IConnection> container,
            IConnectionHandler connectionHandler,
            IServiceProvider provider,
            Socket listener)
        {
            _listener = listener;
            _conContainer = container;
            _serviceProvider = provider;
            _connectionHandler = connectionHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var sock = await _listener.AcceptAsync(stoppingToken);

                var conn = ActivatorUtilities.CreateInstance<IConnection>(_serviceProvider);

                conn.InitalizeNetworker(
                    sock, 
                    _connectionHandler.disconnectEvent, 
                    _connectionHandler.incomingEvent
                );

                _conContainer.TryAdd(sock, conn);
            }
        }
    }
}