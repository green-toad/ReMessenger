using System.Net.Sockets;
using System.Threading.Channels;
using AVcontrol;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDriver.AE;
using MessengerServer.ConnectionReciver;
using MessengerServer.RequestHandler;
using System.Net;
using Shared.Source.AsymEncryptionImpl;
using MessengerServer.CorpseCleaner;

namespace MessengerServer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // example to use database
            //var host = Host.CreateDefaultBuilder(args)
            //    .ConfigureServices((context, services) =>
            //    {
            //        services.AddDbContext<AppDbContext>(options =>
            //            options.UseNpgsql("Host=localhost;Database=JabNetDatabase;Username=Jadmin;Password=4649"));

            //        services.AddTransient<Func<AppDbContext>>(sp => sp.GetRequiredService<AppDbContext>);
            //        services.AddSingleton<Handler>();
            //        services.AddHostedService<Handler>();
            //    })
            //    .Build();

            //using var scope = host.Services.CreateScope();
            //var handler = scope.ServiceProvider.GetRequiredService<Handler>();

            //await host.RunAsync();

            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(new IPEndPoint(IPAddress.Any, 22022));
            sock.Listen();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContextFactory<AppDbContext>
                    (options =>
                        options.UseNpgsql("Host=localhost;Database=JabNetDatabase;Username=Jadmin;Password=4649"
                    ));

                    services.AddSingleton<IHashContainer<ClientInformation>, ConnectionContainer>();
                    services.AddHostedService<ConnectionReceiver>();
                    services.AddSingleton<Socket>(sock);
                    services.AddHostedService<MessageHandler>();
                    services.AddSingleton<IConnectionFabric, ConnectionHandlerFactory>();
                    services.AddHostedService<ConnectionAccepter>();
                    services.AddTransient<IEncryptor, TestImpl>();
                    services.AddHostedService<Cleaner>();
                    services.AddSingleton<IEncryptorFabric, EncryptorFactory>();
                })
            .Build();

            await host.RunAsync();

            Console.ReadKey();
            
            await host.StopAsync(TimeSpan.FromSeconds(5)); 
        }
    }

    // similar example of use database
    //internal class Handler : BackgroundService
    //{
    //    private readonly IServiceScopeFactory _scopeFactory;

    //    public Handler(IServiceScopeFactory scopeFactory)
    //    {
    //        _scopeFactory = scopeFactory;
    //    }


    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(1000 * 30);
    //            await HandleAsync();
    //        }
    //    }

    //    public async Task HandleAsync()
    //    {
    //        using (var scope = _scopeFactory.CreateScope())
    //        {
    //            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //            var last = await context.Messages
    //                .OrderByDescending(m => m.Time)
    //                .FirstOrDefaultAsync();

    //            ulong newId = (last?.SUID ?? 0) + 1;

    //            var msg = new Message
    //            {
    //                SUID = newId,
    //                Time = DateTime.UtcNow,
    //                Owner = 22022,
    //                Membership = 11011,
    //                ContentType = Message.Type.text,
    //                Content = $"its time to tea: its {DateTime.Now}"
    //            };

    //            await context.Messages.AddAsync(msg);
    //            await context.SaveChangesAsync();
    //        }
    //    }
    //  }

    public class TemporaryPlug : IEncryptor
    {
        public Span<byte> Encrypt(Span<byte> content)
        {
            return content;
        }

        public Span<byte> Decrypt(Span<byte> content)
        {
            return content;
        }

        public void Next()
        {
        }

        public void GenerateKey()
        {
        }

        public bool ImportKey(Span<byte> key)
        {
            return true;
        }

        public Span<byte> ExportKey()
        {
            return new Span<byte>();
        }

        public bool IsKeyValid()
        {
            return true;
        }

        public bool IsEncryptedMsgValid()
        {
            return true;
        }

        public List<byte> Encrypt(List<byte> content)
        {
            throw new NotImplementedException();
        }

        public List<byte> Decrypt(List<byte> content)
        {
            throw new NotImplementedException();
        }

        public bool ImportKey(List<byte> key)
        {
            throw new NotImplementedException();
        }

        List<byte> IEncryptor.ExportKey()
        {
            throw new NotImplementedException();
        }
    }
}