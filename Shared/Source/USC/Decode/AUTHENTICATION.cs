using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public (bool sendAuthTokenIfLoginSuccess, UInt64 suid, string password)
            STD_AUTHENTICATION(Byte[] packedContent)
        {
            return
            (
                packedContent[0] > 127,
                FromBinary.LittleEndian<UInt64>(packedContent[1..9]),
                FromBinary.Utf8(packedContent[9..])
            );
        }
        static public (UInt64 suid, Byte[] authToken)
            TOKEN_AUTHENTICATION(Byte[] packedContent)
        {
            return
            (
                FromBinary.LittleEndian<UInt64>(packedContent[0..8]),
                packedContent[8..]
            );
        }
    }
}