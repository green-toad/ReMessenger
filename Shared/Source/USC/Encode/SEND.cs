using System;


using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] SEND_MSG(UInt64 sessionId, string msgText)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.SEND_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.BigEndian(DateTime4b.Now.PassedTotalMinutes),
                    .. ToBinary.Utf8(msgText)
                ]
            );
        }

        static public Byte[] SEND_PIC(UInt64 sessionId)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.SEND_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.BigEndian(DateTime4b.Now.PassedTotalMinutes),
                    //.. ToBinary.BigEndian(picture)
                ]
            );
        }
        static public Byte[] SEND_FILE(UInt64 sessionId)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.SEND_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.BigEndian(DateTime4b.Now.PassedTotalMinutes),
                    //.. ToBinary.BigEndian(fileContentPart)
                ]
            );
        }
        static public Byte[] SEND_MUSIC(UInt64 sessionId)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.SEND_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.BigEndian(DateTime4b.Now.PassedTotalMinutes),
                    //.. ToBinary.BigEndian(music)
                ]
            );
        }
        static public Byte[] DELETE_MSG(UInt64 sessionId, UInt64 messageId)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.DELETE_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                    
                ],
                [
                    .. ToBinary.BigEndian(messageId)
                ]
            );
        }
    }
}