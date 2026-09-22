using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Channels;
using MessengerServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetDriver.AE;

namespace MessengerServer.Implementations
{
    internal class ConnectionHendler : IConnectionHandler
    {
        public DisconnectEvent disconnectEvent => DisconnectSync;

        public IncomingEvent incomingEvent => IncomingEvent;

        private readonly ConcurrentDictionary<Socket, IConnection> _conContainer;
        private readonly CancellationTokenSource _cts;
        private readonly Channel<Socket> _deathQueue;
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly IHashMaker _hasher;
        private readonly Task _killerTask;

        public ConnectionHendler(
            ConcurrentDictionary<Socket, IConnection> container, 
            IDbContextFactory<AppDbContext> dbFactory,
            IHashMaker hasher)
        {
            _conContainer = container;
            _dbFactory = dbFactory;
            _hasher = hasher;

            _deathQueue = Channel.CreateUnbounded<Socket>();
            _killerTask = Task.Run(KillerTask);
            _cts = new();
        }

        private void DisconnectSync(Socket socket)
        {
            _deathQueue.Writer.WriteAsync(socket); // очередь безгранична -- запись мнгновенна
        }

        private async Task KillerTask() // существет, что бы небыло self-await сюрпризов от networker
        {
            await foreach(var socket in _deathQueue.Reader.ReadAllAsync(_cts.Token))
            {
                _conContainer.TryRemove(socket, out _); // соединение трансиентное и реализует диспозабле -- будет уничтожено хостингом
            }
        }

        private async Task IncomingEvent(ResultContent enter)
        {
            
        }

        public async ValueTask DisposeAsync()
        {
            _cts.Cancel();

            await _killerTask;
            _deathQueue.Writer.Complete();

            _cts.Dispose();
        }
    }
}