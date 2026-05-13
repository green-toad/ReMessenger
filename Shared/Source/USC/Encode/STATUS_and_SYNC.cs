using System;
using System.Collections.Generic;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] MESSAGE_STATUS(UInt64 sessionId, MessageStatus messageStatus)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.UPDATE_MESSAGE_STATUS,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [(Byte)messageStatus]
            );
        }

        static public Byte[] PING_STATUS(UInt64 sessionId, PingStatus pingStatus)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.UPDATE_PING_STATUS,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [(Byte)pingStatus]
            );
        }



        static public Byte[] HERE_IS_SYNC(UInt64 sessionId, SubCommand reKeyExportType, List<Byte> reKeyExport)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.HERE_IS_SYNC,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                    reKeyExportType
                ],
                [.. reKeyExport]
            );
        }
        static public Byte[] I_RECEIVED_SYNC(UInt64 sessionId)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.I_RECEIVED_SYNC,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK
                ],
                []
            );
        }

        static public Byte[] TRY_CHANGE_HEAD_DEVICE(UInt64 sessionId, Byte myNewPriority)
        {
            return PackTogether
            (
                sessionId,
                MainCommand.TRY_CHANGE_MY_DEVICE_PRIORITY,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK
                ],
                [
                    myNewPriority
                ]
            );
        }
    }
}