using System;
using System.Collections.Generic;

namespace Shared.Source
{
    public interface IAsymetricEncriptor
    {
        Span<byte> Encrypt(Span<byte> content);
        Span<byte> Decrypt(Span<byte> content);

        void Next();
        void GenerateKey();

        bool ImportKey(Span<byte> key);
        Span<byte> ExportKey();

        bool IsKeyValid();
        bool IsEncryptedMsgValid();
    }
}