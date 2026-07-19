using MessengerServer.RequestHandler;
using Microsoft.Extensions.Hosting;

namespace MessengerServer.CorpseCleaner
{
    internal class Cleaner : BackgroundService
    {
        private readonly int _timeout = 5000;
        private readonly IHashContainer<ClientInformation> _container;
        public Cleaner(IHashContainer<ClientInformation> container)
        {
            _container = container;
        }
        protected override async 
        Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_timeout);

                foreach (var clientInfo in _container.GetAll())
                {
                    if (!clientInfo.networker.alive)
                    {
                        _container.Pop(clientInfo);
                    }
                }
            }
        }
    }
}