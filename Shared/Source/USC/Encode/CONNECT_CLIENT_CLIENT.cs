using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] CONNECT_CLIENT_CLIENT_1_EN(UInt64 companionSUID,
            Encryptors.AsymetricEncryptionType aencType, Byte[] publicKey)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_CLIENT_1_EN,
                .. ToBinary.LittleEndian(companionSUID),
                (Byte) aencType,
                .. publicKey
            ];
        }
        static public Byte[] CONNECT_CLIENT_CLIENT_2_EE(Byte[] publicKey, Byte[] reKeyExport)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_CLIENT_2_EE,
                .. ToBinary.LittleEndian<Int32>(publicKey.Length),
                .. publicKey,
                .. reKeyExport
            ];
        }
        static public Byte[] CONNECT_CLIENT_CLIENT_3_NE(UInt64 mySUID, Byte[] reKeyExport)
        {
            return
            [
                (Byte) MainCommand.CONNECT_CLIENT_CLIENT_3_NE,
                .. ToBinary.LittleEndian(mySUID),
                .. reKeyExport
            ];
        }
    }
}