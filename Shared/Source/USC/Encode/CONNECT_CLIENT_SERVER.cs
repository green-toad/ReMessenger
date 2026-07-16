using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] CONNECT_CLIENT_SERVER_1_EN(Encryptors.AsymetricEncryptionType aencType, Byte[] publicKey)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_SERVER_1_EN,
                (Byte) aencType,
                .. publicKey
            ];
        }
        static public Byte[] CONNECT_CLIENT_SERVER_2_EE(Byte[] publicKey, Byte[] reKeyExport)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_SERVER_2_EE,
                .. ToBinary.LittleEndian(publicKey.Length),
                .. publicKey,
                .. reKeyExport
            ];
        }
        static public Byte[] CONNECT_CLIENT_SERVER_3_NE(Byte[] reKeyExport)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_SERVER_3_NE,
                .. reKeyExport
            ];
        }
    }
}