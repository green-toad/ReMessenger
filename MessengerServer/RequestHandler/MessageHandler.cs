using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using NetDriver.AE;
using System.Threading.Channels;
using Shared.Source.USC;
using Microsoft.EntityFrameworkCore;
using MessengerServer.AccauntManagment;

namespace MessengerServer.RequestHandler
{
    internal class MessageHandler : BackgroundService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly IHashContainer<ClientInformation> _container;
        private readonly IHashMaker _hashMaker;
        public MessageHandler(IDbContextFactory<AppDbContext> dbFactory, IHashContainer<ClientInformation> container, IHashMaker hasher)
        {
            _dbFactory = dbFactory;
            _container = container;
            _hashMaker = hasher;
        }

        private readonly Channel<ResultContent> _processingRequests = Channel.CreateBounded<ResultContent>(15 * 100);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var req in _processingRequests.Reader.ReadAllAsync())
            {
                var package = Decode.UnPack(req.content);
                
                var client = _container.Get(req.socket);

                await using var datacontext = await _dbFactory.CreateDbContextAsync(stoppingToken);

                switch (package.MainCommand)
                {
                    case MainCommand.I_REQUEST_ACTIVE_CHATS:
                        if (req.type == ResultContent.Type.from)
                        {
                            var chts = await datacontext.Participants
                                .Where(p => p.UserSUID == client.suid)
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
                            client.networker.Answer(Encode.HERE_IS_ACTIVE_CHATS(package.ForResponseSID, chts.ToArray()), req.frameuid.Value);
                        }
                        break;
                    
                    case MainCommand.I_REQUEST_CHAT_HISTORY_UPDATE:
                        if (req.type == ResultContent.Type.from)
                        {
                            var cntntFchStRsp = Decode.I_REQUEST_CHAT_HISTORY_UPDATE(package.PackedContent);
                            client.networker.Answer(Encode.HERE_IS_CHAT_HISTORY_UPDATE(await datacontext.Messages.Where(m => m.Membership == cntntFchStRsp.ChatSuid).ToArrayAsync(), 
                            package.SessionId, 
                            package.ForResponseSID), 
                            req.frameuid.Value);
                        }
                        break;
                    
                    case MainCommand.SEND_MSG:
                        var cntntFmsgSnd = Decode.SEND_MSG(package.PackedContent);
                        await datacontext.Messages.AddAsync(new Message(cntntFmsgSnd.sentTime, cntntFmsgSnd.message, cntntFmsgSnd.authorSUID, cntntFmsgSnd.membership, cntntFmsgSnd.messageSUID), stoppingToken);
                        break;
                    
                    case MainCommand.SEND_PIC:
                        var cntntFpicSnd = Decode.SEND_MSG(package.PackedContent);
                        await datacontext.Messages.AddAsync(new Message(cntntFpicSnd.sentTime, cntntFpicSnd.message, cntntFpicSnd.authorSUID, cntntFpicSnd.membership, cntntFpicSnd.messageSUID), stoppingToken);
                        break;
                    
                    case MainCommand.SEND_FILE:
                        var cntntFfileSnd = Decode.SEND_FILE(package.PackedContent);
                        await datacontext.Messages.AddAsync(new Message(cntntFfileSnd.sentTime, cntntFfileSnd.message, cntntFfileSnd.authorSUID, cntntFfileSnd.membership, cntntFfileSnd.messageSUID), stoppingToken);
                        break;

                    case MainCommand.SEND_MUSIC:
                        var cntntFmusicSnd = Decode.SEND_MUSIC(package.PackedContent);
                        await datacontext.Messages.AddAsync(new Message(cntntFmusicSnd.sentTime, cntntFmusicSnd.message, cntntFmusicSnd.authorSUID, cntntFmusicSnd.membership, cntntFmusicSnd.messageSUID), stoppingToken);
                        break;

                    case MainCommand.DELETE_MSG:
                        var cntntFdel = Decode.DELETE_MSG(package.PackedContent);
                        var msgToRemove = datacontext.Messages.FirstOrDefault(m => m.messageSUID == cntntFdel);
                        if (msgToRemove != null)
                            datacontext.Messages.Remove(msgToRemove);
                        break;

                    case MainCommand.STD_AUTHENTICATION:
                        var cntntFaut = Decode.STD_AUTHENTICATION(package.PackedContent);
                        var usrFreg = _container.Get(req.socket);
                        var usrFdbFreg = await datacontext.Users.FindAsync(cntntFaut.suid, stoppingToken);
                        if (usrFdbFreg != null)
                        {
                            if (_hashMaker.IsCorrect(cntntFaut.password, usrFdbFreg))
                            {
                                usrFreg.suid = cntntFaut.suid;
                            }
                        }
                        break;
                }
                await datacontext.SaveChangesAsync(stoppingToken);
                await datacontext.DisposeAsync();
            }
        }

        public async Task Incoming(ResultContent result)
        {
            await _processingRequests.Writer.WriteAsync(result);
        }
    }
}
