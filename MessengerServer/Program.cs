using System.Net.Sockets;
using System.Threading.Channels;
using AVcontrol;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using Shared.Source.Encryptors;
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

                    services.AddSingleton<IHashMaker, HashMaker>();
                })
            .Build();

            await host.RunAsync();

            Console.ReadKey();
            
            await host.StopAsync(TimeSpan.FromSeconds(5)); 
        }
    }
}
/*
два синглтона фабрика и сервис

сервис на подключение

фабрика соеденений с postgre

синглтон -- обработчик

синглтон -- контеинер подключений

трансиент -- экземпляры подключений (внутри храним айдишники, нетворкеры, всю хурму)
*/