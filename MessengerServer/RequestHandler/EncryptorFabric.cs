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
        public IAsymetricEncryptor Create();
    }


    public class EncryptorFactory : IEncryptorFabric
    {
        private readonly IServiceProvider _serviceProvider;

        public EncryptorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAsymetricEncryptor Create()
        {
            return ActivatorUtilities.CreateInstance<IAsymetricEncryptor>(_serviceProvider);
        }
    }
}
