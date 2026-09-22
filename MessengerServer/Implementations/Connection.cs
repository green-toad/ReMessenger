using System.Net.Sockets;
using MessengerServer.Interfaces;
using NetDriver.AE;

namespace MessengerServer.Implementations
{
    internal class Connection : IConnection
    {
        private bool _isSUIDset;
        private Guid _mySuid;
        private Networker _myNetworker;
        private readonly IAsymetrycEncryptor _myAsymEncryptor;
        private readonly ISymetrycEncryptor _mySymEncryptor;
        public Guid USUID => _mySuid;

        public Networker UNetworker => _myNetworker;

        public IAsymetrycEncryptor UAsymEncryptor => _myAsymEncryptor;

        public ISymetrycEncryptor USymEncryptor => _mySymEncryptor;

        public void InitalizeNetworker(Socket socket, DisconnectEvent devent, IncomingEvent ievent)
        {
            _myNetworker = new(socket, ievent, devent);
        }

        public bool InitalizeSUID(Guid suid)
        {
            if (_isSUIDset) return false;
            _mySuid = suid;
            return true; 
        }

        public async ValueTask DisposeAsync()
        {
            try{
                await _myNetworker.Dispose();
            }
            catch {}
        }

        public Connection(IAsymetrycEncryptor ase, ISymetrycEncryptor sye)
        {
            _myAsymEncryptor = ase;
            _mySymEncryptor = sye;
            _isSUIDset = false;
        }
    }
}