using MessengerServer.RequestHandler;
using Microsoft.Extensions.Hosting;

namespace MessengerServer.CorpseCleaner
{
    internal class Cleaner : BackgroundService
    {
        private readonly int _timeout = 5000;
        private readonly IConnectionContainer _container;
        public Cleaner(IConnectionContainer container)
        {
            _container = container;
        }
        protected override async 
        Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_timeout);

                foreach (var connection in _container.GetAllWorkers())
                {
                    if (!connection.alive)
                    {
                        _container.Pop(connection);
                    }
                }
                _container.Sync();
            }
        }
    }
}