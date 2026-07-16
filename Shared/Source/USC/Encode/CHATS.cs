using System;
using System.Text;


using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] I_REQUEST_ACTIVE_CHATS(UInt64 sessionId, UInt64 forResponseSID)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.I_REQUEST_ACTIVE_CHATS,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK
                ],
                []
            );
        }
        static public Byte[] HERE_IS_ACTIVE_CHATS(UInt64 responseSID, JN_Chat[] chats)
        {
            Int32 totalLength = 0;
            foreach (var chat in chats)
            {
                Int32 avatarBytes = Encoding.Unicode.GetByteCount(chat.chatAvatar);
                Int32 nameBytes   = Encoding.Unicode.GetByteCount(chat.name);
                Int32 bioBytes    = Encoding.Unicode.GetByteCount(chat.bio);
                Int32 membersCount = chat.membersSUID.Count;
                totalLength += 2 + 2 + 8 + 2 + nameBytes + 2 + bioBytes + avatarBytes + 8 * membersCount;
            }

            Byte[] result = new Byte[totalLength];
            Int32 offset = 0;

            foreach (var chat in chats)
            {
                Int32 avatarBytes = Encoding.Unicode.GetByteCount(chat.chatAvatar);
                Int32 nameBytes   = Encoding.Unicode.GetByteCount(chat.name);
                Int32 bioBytes    = Encoding.Unicode.GetByteCount(chat.bio);
                Int32 membersCount = chat.membersSUID.Count;
                Buffer.BlockCopy(ToBinary.LittleEndian<UInt16>((UInt16)avatarBytes), 0, result, offset, 2);
                offset += 2;
                Buffer.BlockCopy(ToBinary.LittleEndian<UInt16>((UInt16)membersCount), 0, result, offset, 2);
                offset += 2;
                Buffer.BlockCopy(ToBinary.LittleEndian<UInt64>(chat.chatSUID), 0, result, offset, 8);
                offset += 8;

                Buffer.BlockCopy(ToBinary.LittleEndian<UInt16>((UInt16)nameBytes), 0, result, offset, 2);
                offset += 2;
                Byte[] nameData = ToBinary.Utf16(chat.name);
                Buffer.BlockCopy(nameData, 0, result, offset, nameBytes);
                offset += nameBytes;
                Buffer.BlockCopy(ToBinary.LittleEndian<UInt16>((UInt16)bioBytes), 0, result, offset, 2);
                offset += 2;

                Byte[] bioData = ToBinary.Utf16(chat.bio);
                Buffer.BlockCopy(bioData, 0, result, offset, bioBytes);
                offset += bioBytes;

                Byte[] avatarData = ToBinary.Utf16(chat.chatAvatar);
                Buffer.BlockCopy(avatarData, 0, result, offset, avatarBytes);
                offset += avatarBytes;

                foreach (var suid in chat.membersSUID)
                {
                    Buffer.BlockCopy(ToBinary.LittleEndian<UInt64>(suid), 0, result, offset, 8);
                    offset += 8;
                }
            }

            return PackTogether
            (
                responseSID,
                0,
                MainCommand.HERE_IS_ACTIVE_CHATS,
                [],
                result
            );
        }



        static public Byte[] I_REQUEST_CHAT_HISTORY_UPDATE(UInt64 sessionId, UInt64 forResponseSID, UInt32 fromMessageSUID, UInt32 chatSUID)
        {
            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.I_REQUEST_CHAT_HISTORY_UPDATE,
                [
                    SubCommand.SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK
                ],
                [
                    .. ToBinary.LittleEndian(fromMessageSUID),
                    .. ToBinary.LittleEndian(chatSUID)
                ]
            );
        }


        static public Byte[] HERE_IS_CHAT_HISTORY_UPDATE(JN_Message[] chatStory, UInt64 sessionId, UInt64 forResponseSID)
        {
            Int32 totalLength = 0;
            foreach (var msg in chatStory)
            {
                totalLength += 4 + 8 + Encoding.Unicode.GetByteCount(msg.message) + 8 + 8 + 4;
            }
            Byte[] result = new Byte[totalLength];
            Int32 offset = 0;

            foreach (var msg in chatStory)
            {
                Int32 msgLen = Encoding.Unicode.GetByteCount(msg.message);
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
                offset += 4;
            }

            return PackTogether
            (
                sessionId,
                forResponseSID,
                MainCommand.HERE_IS_CHAT_HISTORY_UPDATE,
                [],
                result
            );
        }
    }
}