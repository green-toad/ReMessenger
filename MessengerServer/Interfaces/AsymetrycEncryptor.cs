namespace MessengerServer.Interfaces
{
    internal interface IAsymetrycEncryptor
    {
        byte[] GetPublicKey();
        void ComputeSharedSecret(byte[] otherPkey);
        byte[] DeriveKey(string salt, int keyLength);
        byte[] EncryptData(byte[] data);
        byte[] DecryptData(byte[] encryptedPackage);
    }
}