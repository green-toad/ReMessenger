using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public (Encryptors.AsymetricEncryptionType aencType, Byte[] publicKey)
            CONNECT_CLIENT_SERVER_1_EN(Byte[] packedContent)
        {
            Byte unsanitizedAencType = packedContent[1];

            return
            (
                Enum.IsDefined(typeof(Encryptors.AsymetricEncryptionType), unsanitizedAencType)
                    ? (Encryptors.AsymetricEncryptionType)unsanitizedAencType
                    :  Encryptors.AsymetricEncryptionType.UNKNOWN,
                packedContent[2..]
            );
        }
        static public (Byte[] publicKey, Byte[] reKeyExport) CONNECT_CLIENT_SERVER_2_EE(Byte[] packedContent)
        {
            Int32 publicKeyLength = FromBinary.LittleEndian<Int32>(packedContent[0..5]);

            Byte[] publicKey = packedContent[5..(5 + publicKeyLength)];
            Byte[] reKeyExport = packedContent[(5 + publicKeyLength)..];

            return (publicKey, reKeyExport);
        }
    }
}