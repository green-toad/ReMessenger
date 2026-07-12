using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDriver.AE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

namespace MessengerServer.ConnectionReciver
{
    public class ConnectionAccepter : BackgroundService
    {
        public readonly Channel<Socket> acceptingRequests = Channel.CreateBounded<Socket>(500);
        private readonly Socket _socket;
        public ConnectionAccepter(Socket socket)
        {
            _socket = socket;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await acceptingRequests.Writer.WriteAsync(await _socket.AcceptAsync(stoppingToken), stoppingToken);
            }
        }
    }
}
