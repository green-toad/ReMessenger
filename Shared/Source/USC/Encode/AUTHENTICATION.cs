using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] STD_AUTHENTICATION(UInt64 sessionId, UInt64 forResponseSID, bool sendAuthTokenIfLoginSuccess, UInt64 suid, string password)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.STD_AUTHENTICATION,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    (Byte) (sendAuthTokenIfLoginSuccess ? 1 : 0),
                    .. ToBinary.LittleEndian(suid),
                    .. ToBinary.Utf8(password),
                ]
            );
        }
        static public Byte[] TOKEN_AUTHENTICATION(UInt64 sessionId, UInt64 forResponseSID, UInt64 suid, Byte[] authToken)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.TOKEN_AUTHENTICATION,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.LittleEndian(suid),
                    .. authToken,
                ]
            );
        }
        static public Byte[] HERE_IS_TOKEN_FOR_NEXT_AUTH(UInt64 sessionId, UInt64 forResponseSID, Byte[] authToken)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.HERE_IS_TOKEN_FOR_NEXT_AUTH,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                authToken
            );
        }
    }
}