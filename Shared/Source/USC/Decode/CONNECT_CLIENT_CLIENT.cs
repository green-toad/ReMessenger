using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public (UInt64 companionSUID,
                       Encryptors.AsymetricEncryptionType aencType,
                       Byte[] publicKey)
            CONNECT_CLIENT_CLIENT_1_EN(Byte[] packedContent)
        {
            Byte unsanitizedAencType = packedContent[9];

            return
            (
                FromBinary.LittleEndian<UInt64>(packedContent[1..9]),
                Enum.IsDefined(typeof(Encryptors.AsymetricEncryptionType), unsanitizedAencType)
                    ? (Encryptors.AsymetricEncryptionType)unsanitizedAencType
                    :  Encryptors.AsymetricEncryptionType.UNKNOWN,
                packedContent[10..]
            );
        }
        static public (Byte[] publicKey, Byte[] reKeyExport) CONNECT_CLIENT_CLIENT_2_EE(Byte[] packedContent)
        {
            Int32 publicKeyLength = FromBinary.LittleEndian<Int32>(packedContent[0..5]);

            return
            (
                packedContent[5..(5 + publicKeyLength)],
                packedContent[(5 + publicKeyLength)..]
            );
        }
    }
}