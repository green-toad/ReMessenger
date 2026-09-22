using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using MessengerServer.Interfaces;

namespace MessengerServer.Implementations
{
    internal class X25519_Device : IAsymetrycEncryptor
        {
        private readonly AsymmetricKeyParameter? _privateKey;
        private readonly AsymmetricKeyParameter? _publicKey;
        private readonly X25519KeyPairGenerator _generator;
        private readonly X25519Agreement _keyAgreement;
        private byte[] SharedSecret;

        #pragma warning disable CS8618 // так надо
        public X25519_Device()
        {
            _generator = new X25519KeyPairGenerator();
            _generator.Init(new KeyGenerationParameters(new SecureRandom(), 256));

            var keyPair = _generator.GenerateKeyPair();
            _privateKey = keyPair.Private;
            _publicKey = keyPair.Public;
            _keyAgreement = new X25519Agreement();
        }
        #pragma warning restore CS8618

        public byte[] GetPublicKey()
        {
            if (_publicKey is not X25519PublicKeyParameters publicKey)
                throw new InvalidOperationException("Публичный ключ странный");
            
            return publicKey.GetEncoded();
        }
        public void ComputeSharedSecret(byte[] otherPkey)
        {
            if (_privateKey is not X25519PrivateKeyParameters privateKey)
                throw new InvalidOperationException("Приватный ключ странный");

            var otherPublicKey = new X25519PublicKeyParameters(otherPkey, 0);
            
            
            _keyAgreement.Init(privateKey);
            
            SharedSecret = new byte[_keyAgreement.AgreementSize];
            _keyAgreement.CalculateAgreement(otherPublicKey, SharedSecret, 0);
        }
        public byte[] DeriveKey(string salt, int keyLength)
        {
            using var hmac = new HMACSHA256(SharedSecret);
            
            var derived = new byte[keyLength];
            int offset = 0;
            int counter = 0;
            while (offset < keyLength)
            {
                var input = System.Text.Encoding.UTF8.GetBytes(salt + counter++);
                var hash = hmac.ComputeHash(input);
                var take = Math.Min(hash.Length, keyLength - offset);
                Array.Copy(hash, 0, derived, offset, take);
                offset += take;
            }
            return derived;
        }
        public byte[] EncryptData(byte[] data)
        {
            byte[] key = DeriveKey("there_is_nothing_to_see", 32);
            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            byte[] ciphertext = new byte[data.Length];
            byte[] tag = new byte[16];

            using var aes = new AesGcm(key);
            aes.Encrypt(nonce, data, ciphertext, tag);

            var result = new byte[nonce.Length + tag.Length + ciphertext.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
            Buffer.BlockCopy(ciphertext, 0, result, nonce.Length + tag.Length, ciphertext.Length);
            return result;
        }
        public byte[] DecryptData(byte[] encryptedPackage)
        {
            byte[] key = DeriveKey("there_is_nothing_to_see", 32);
            int nonceLen = 12, tagLen = 16;
            byte[] nonce = new byte[nonceLen];
            byte[] tag = new byte[tagLen];
            byte[] ciphertext = new byte[encryptedPackage.Length - nonceLen - tagLen];

            Buffer.BlockCopy(encryptedPackage, 0, nonce, 0, nonceLen);
            Buffer.BlockCopy(encryptedPackage, nonceLen, tag, 0, tagLen);
            Buffer.BlockCopy(encryptedPackage, nonceLen + tagLen, ciphertext, 0, ciphertext.Length);

            byte[] plaintext = new byte[ciphertext.Length];
            using var aes = new AesGcm(key);
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
            return plaintext;
        }
    }
}