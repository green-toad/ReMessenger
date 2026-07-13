using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using NetDriver.AE;
using System.Threading.Channels;
using Shared.Source.USC;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace MessengerServer.RequestHandler
{
    internal class MessageHandler : BackgroundService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly IConnectionContainer _container;
        public MessageHandler(IDbContextFactory<AppDbContext> dbFactory, IConnectionContainer container)
        {
            _dbFactory = dbFactory;
            _container = container;
        }

        private readonly Channel<ResultContent> _processingRequests = Channel.CreateBounded<ResultContent>(15 * 100);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var req in _processingRequests.Reader.ReadAllAsync())
            {
                var package = Decode.UnPack(req.content);
                
                var ntwrkr = _container.GetNetworker(req.socket);

                var uid = _container.GetSuid(req.socket);

                await using var datacontext = await _dbFactory.CreateDbContextAsync(stoppingToken);

                switch (package.MainCommand)
                {
                    case MainCommand.I_REQUEST_ACTIVE_CHATS:
                        if (req.type == ResultContent.Type.from)
                        {
                            var chts = await datacontext.Participants
                                .Where(p => p.UserSUID == uid)
                                .Include(p => p.ChatLink)
                                    .ThenInclude(c => c.Participants)
                                .Select(p => p.ChatLink)
                                .Distinct()
                                .ToListAsync();
                            foreach (var chat in chts)
                            {
                                chat.membersSUID = chat.Participants
                                                    .Select(p => p.UserSUID)
                                                    .ToList();
                            }
                            ntwrkr.Answer(Encode.PackTogether(
                                package.SessionId,
                                package.ForResponseSID,
                                MainCommand.HERE_IS_ACTIVE_CHATS,
                                [],
                                Encode.HERE_IS_ACTIVE_CHATS(
                                    chts.ToArray()
                                )
                            ), 
                            req.frameuid.Value
                            );
                        }
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
