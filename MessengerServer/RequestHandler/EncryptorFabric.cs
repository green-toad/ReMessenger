using Microsoft.Extensions.DependencyInjection;
using NetDriver.AE;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Shared.Source.AsymEncryptionImpl;

namespace MessengerServer.ConnectionReciver
{
    public interface IEncryptorFabric
    {
        public IEncryptor Create();
    }


    public class EncryptorFactory : IEncryptorFabric
    {
        private readonly IServiceProvider _serviceProvider;

        public EncryptorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEncryptor Create()
        {
            return ActivatorUtilities.CreateInstance<IEncryptor>(_serviceProvider);
        }
    }
}
