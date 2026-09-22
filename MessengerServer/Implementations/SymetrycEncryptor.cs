using MessengerServer.Interfaces;

namespace MessengerServer.Implementations
{
    internal class SymCryptoDevice() : ISymetrycEncryptor
    {
        private readonly JabrAPI.RE5.ReKey _key = new();

        public byte[] exportKey() => _key.ExportAsBinary();
        public void importKey(byte[] key) => _key.ImportFromBinary(key);

        public byte[] Encrypt(byte[] content) => [.. JabrAPI.RE5.EncryptData.WithValidationAndNoising([.. content], _key).result];
        public byte[] Decript(byte[] content) => [.. JabrAPI.RE5.DecryptData.WithValidationAndDeNoising([.. content], _key, true)];
    }
}