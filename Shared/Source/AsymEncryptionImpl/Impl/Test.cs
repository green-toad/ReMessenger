using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AVcontrol;
using JabrAPI;

namespace Shared.Source.AsymEncryptionImpl
{
    public class TestImpl : IAsymetricEncryptor
    {
        RE5.BinaryKey binKey;
        public List<byte> Encrypt(List<byte> content)
        {
            return RE5.Encrypt.WithNoise.Bytes(content, binKey);
        }

        public List<byte> Decrypt(List<byte> content)
        {
            return RE5.Decrypt.WithNoise.Bytes(content, binKey);
        }

        public void Next()
        {
            binKey.Next();
        }

        public void GenerateKey()
        {
            binKey = new();
        }

        public bool ImportKey(List<byte> key)
        {
            try
            {
                binKey = new(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<byte> ExportKey()
        {
            return binKey.ExportAsBinary();
        }

        public bool IsKeyValid()
        {
            return true;
        }

        public bool IsEncryptedMsgValid()
        {
            return true;
        }
    }
}