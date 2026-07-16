using System;
using System.Text;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] SEND_MSG(UInt64 sessionId, UInt64 forResponseSID, JN_Message msg)
        {
            Int32 msgLen = Encoding.Unicode.GetByteCount(msg.message);
            Byte[] result = new Byte[4 + 8 + msgLen + 8 + 8 + 4];
            Int32 offset = 0;
            Buffer.BlockCopy(ToBinary.LittleEndian(msgLen), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalMinutes), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalHours), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.Utf16(msg.message), 0, result, offset, msgLen);
            offset += msgLen;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.authorSUID), 0, result, offset, 8);
            offset += 8;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.membership), 0, result, offset, 8);
            offset += 8;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.messageSUID), 0, result, offset, 4);

            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.SEND_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                result
            );
        }

        static public Byte[] SEND_PIC(UInt64 sessionId, UInt64 forResponseSID, JN_Message msg)
        {
            Byte[] result = new Byte[4 + 8 + Encoding.Unicode.GetByteCount(msg.message) + 8 + 4];
            Int32 offset = 0;
            Buffer.BlockCopy(ToBinary.LittleEndian(Encoding.Unicode.GetByteCount(msg.message)), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalMinutes), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalHours), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.Utf16(msg.message), 0, result, offset, Encoding.Unicode.GetByteCount(msg.message));
            offset += Encoding.Unicode.GetByteCount(msg.message);
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.authorSUID), 0, result, offset, 8);
            offset += 8;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.messageSUID), 0, result, offset, 4);
            
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.SEND_PIC,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                result
            );
        }
        static public Byte[] SEND_FILE(UInt64 sessionId, UInt64 forResponseSID, JN_Message msg)
        {
            Byte[] result = new Byte[4 + 8 + Encoding.Unicode.GetByteCount(msg.message) + 8 + 4];
            Int32 offset = 0;
            Buffer.BlockCopy(ToBinary.LittleEndian(Encoding.Unicode.GetByteCount(msg.message)), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalMinutes), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalHours), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.Utf16(msg.message), 0, result, offset, Encoding.Unicode.GetByteCount(msg.message));
            offset += Encoding.Unicode.GetByteCount(msg.message);
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.authorSUID), 0, result, offset, 8);
            offset += 8;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.messageSUID), 0, result, offset, 4);

            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.SEND_FILE,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                result
            );
        }
        static public Byte[] SEND_MUSIC(UInt64 sessionId, UInt64 forResponseSID, JN_Message msg)
        {
            Byte[] result = new Byte[4 + 8 + Encoding.Unicode.GetByteCount(msg.message) + 8 + 4];
            Int32 offset = 0;
            Buffer.BlockCopy(ToBinary.LittleEndian(Encoding.Unicode.GetByteCount(msg.message)), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalMinutes), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.sentTime.PassedTotalHours), 0, result, offset, 4);
            offset += 4;
            Buffer.BlockCopy(ToBinary.Utf16(msg.message), 0, result, offset, Encoding.Unicode.GetByteCount(msg.message));
            offset += Encoding.Unicode.GetByteCount(msg.message);
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.authorSUID), 0, result, offset, 8);
            offset += 8;
            Buffer.BlockCopy(ToBinary.LittleEndian(msg.messageSUID), 0, result, offset, 4);

            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.SEND_MUSIC,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                result
            );
        }
        static public Byte[] DELETE_MSG(UInt64 sessionId, UInt64 forResponseSID, UInt64 messageId)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.DELETE_MSG,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK,
                ],
                [
                    .. ToBinary.LittleEndian(messageId)
                ]
            );
        }
    }
}