using System.Net.Sockets;
using System.Threading.Channels;

using AVcontrol;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Net;

using MessengerServer.Interfaces;
using MessengerServer.Implementations;
using MessengerServer.Serices;

using System.Collections.Concurrent;

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

                    services.AddSingleton<IHashMaker, HashMaker>();
                    services.AddSingleton<ConcurrentDictionary<Socket, IConnection>>();
                    services.AddSingleton<IConnectionHandler, ConnectionHendler>();
                    services.AddSingleton(sock);

                    services.AddTransient<IConnection, Connection>();
                    services.AddTransient<IAsymetrycEncryptor, X25519_Device>();
                    services.AddTransient<ISymetrycEncryptor, SymCryptoDevice>();

                    services.AddHostedService<ConnectionAccepter>();
                })
            .Build();

            await host.RunAsync();

            Console.ReadKey();
            
            await host.StopAsync(TimeSpan.FromSeconds(5)); 
        }
    }
}
/*
четыре синглтона фабрика и сервис и три трнасиента

сервис на подключение

фабрика соеденений с postgre

синглтон -- обработчик

синглтон -- конкурентный словарь -- контеинер подключений

синглтон -- хеш мейкер

синглтон -- основной серверный сокет слушатель

трансиент -- экземпляры подключений (внутри храним айдишники, нетворкеры, всю хурму)

сервис подключения принимает входящие и закидывает в контейнер,
из контейнера входящие обробатывает синглтон обработчика,
для обработки из контейнера вынимается трансиент подключения, в котором хранится вся инфа
*/