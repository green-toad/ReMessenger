using System;



namespace Shared.Source.Encryptors
{
    public enum AsymetricEncryptionType
    {
        RSA = 0,
        REa = 1,
        ECC = 2
    }



    public interface IEncryptorDevice
    {
        Byte[] Encrypt(Byte[] content);
        Byte[] Decrypt(Byte[] content);


        void GenerateKey();

        void SetCustomSettings();


        bool   ImportKey(Byte[] key);
        Byte[] ExportKey();


        bool IsKeyValid();
        bool IsEncryptedMsgValid();
    }
}