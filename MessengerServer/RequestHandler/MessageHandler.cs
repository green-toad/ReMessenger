using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using NetDriver.AE;
using System.Threading.Channels;

namespace MessengerServer.RequestHandler
{
    internal class MessageHandler : BackgroundService
    {
        private readonly Channel<ResultContent> _processingRequests = Channel.CreateBounded<ResultContent>(15 * 100);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var req in _processingRequests.Reader.ReadAllAsync())
            {
                Console.WriteLine("активная работа над задачей!!!!");
            }
        }

        public async Task Incoming(ResultContent result)
        {
            await _processingRequests.Writer.WriteAsync(result);
        }
    }
}
