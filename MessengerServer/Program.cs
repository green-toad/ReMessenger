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
using MessengerServer.AccauntManagment;

namespace MessengerServer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
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

                    services.AddSingleton<IHashMaker, HashMaker>();
                })
            .Build();

            await host.RunAsync();

            Console.ReadKey();
            
            await host.StopAsync(TimeSpan.FromSeconds(5)); 
        }
    }
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