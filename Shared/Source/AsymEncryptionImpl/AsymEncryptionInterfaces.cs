using System;
using System.Collections.Generic;



namespace Shared.Source.AsymEncryptionImpl
{
    public interface IEncryptor
    {
        List<Byte> Encrypt(List<Byte> content);
        List<Byte> Decrypt(List<Byte> content);


        void Next();
        void GenerateKey();


        bool ImportKey(List<Byte> key);
        List<Byte> ExportKey();


        bool IsKeyValid();
        bool IsEncryptedMsgValid();
    }
}