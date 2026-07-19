using Microsoft.Extensions.DependencyInjection;
using NetDriver.AE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Shared.Source.Encryptors;

namespace MessengerServer.ConnectionReciver
{
    public interface IEncryptorFabric
    {
        public IEncryptorDevice Create();
    }


    public class EncryptorFactory : IEncryptorFabric
    {
        private readonly IServiceProvider _serviceProvider;

        public EncryptorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEncryptorDevice Create()
        {
            return ActivatorUtilities.CreateInstance<IEncryptorDevice>(_serviceProvider);
        }
    }
}
