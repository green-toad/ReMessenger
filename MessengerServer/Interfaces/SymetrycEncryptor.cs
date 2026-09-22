namespace MessengerServer.Interfaces
{
    internal interface ISymetrycEncryptor
    {
        byte[] exportKey();
        void importKey(byte[] key);
        byte[] Encrypt(byte[] content);
        byte[] Decript(byte[] content);
    }
}