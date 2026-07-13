using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using NetDriver.AE;
using System.Threading.Channels;
using Shared.Source.USC;
using Microsoft.EntityFrameworkCore;

namespace MessengerServer.RequestHandler
{
    internal class MessageHandler : BackgroundService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        public MessageHandler(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private readonly Channel<ResultContent> _processingRequests = Channel.CreateBounded<ResultContent>(15 * 100);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var req in _processingRequests.Reader.ReadAllAsync())
            {
                var package = Decode.UnPack(req.content);
                
                await using var datacontext = await _dbFactory.CreateDbContextAsync(stoppingToken);

                switch (package.MainCommand)
                {
                    case MainCommand.I_REQUEST_ACTIVE_CHATS:
                        
                        break;
                    
                    case MainCommand.I_REQUEST_CHAT_HISTORY_UPDATE:
                        break;
                    
                    case MainCommand.SEND_MSG:
                        break;
                    
                    case MainCommand.SEND_PIC:
                        break;
                    
                    case MainCommand.SEND_FILE:
                        break;
                    
                    case MainCommand.SEND_MUSIC:
                        break;
                    
                    case MainCommand.DELETE_MSG:
                        break;
                }
            }
        }

        public async Task Incoming(ResultContent result)
        {
            await _processingRequests.Writer.WriteAsync(result);
        }
    }
}
