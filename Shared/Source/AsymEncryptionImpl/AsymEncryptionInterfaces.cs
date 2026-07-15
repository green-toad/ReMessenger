using System;



namespace Shared.Source.AsymEncryptionImpl
{
    public interface IAsymetricEncryptor
    {
        Span<Byte> Encrypt(Span<Byte> content);
        Span<Byte> Decrypt(Span<Byte> content);


        void Next();
        void GenerateKey();


        bool ImportKey(Span<Byte> key);
        Span<Byte> ExportKey();


        bool IsKeyValid();
        bool IsEncryptedMsgValid();
    }
}