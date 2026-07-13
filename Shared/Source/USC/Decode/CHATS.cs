using System;
using System.Collections.Generic;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public Byte[] I_REQUEST_ACTIVE_CHATS(byte[] packedContent)
        {
            return packedContent;
        }
        static public List<JN_Chat> HERE_IS_ACTIVE_CHATS(byte[] packedContent)
        {
            int offset = 0;
            List<JN_Chat> listchat = new();

            while (offset < packedContent.Length)
            {
                ushort lenA = FromBinary.LittleEndian<UInt16>(packedContent.AsSpan(offset, 2));
                offset += 2;
                ushort lenM = FromBinary.LittleEndian<UInt16>(packedContent.AsSpan(offset, 2));
                offset += 2;
                ulong chsuid = FromBinary.LittleEndian<UInt64>(packedContent.AsSpan(offset, 8));
                offset += 8;
                ushort lenName = FromBinary.LittleEndian<UInt16>(packedContent.AsSpan(offset, 2));
                offset += 2;
                string name = FromBinary.Utf16(packedContent.AsSpan(offset, lenName));
                offset += lenName;
                ushort lenBio = FromBinary.LittleEndian<UInt16>(packedContent.AsSpan(offset, 2));
                offset += 2;
                string bio = FromBinary.Utf16(packedContent.AsSpan(offset, lenBio));
                offset += lenBio;
                string path = FromBinary.Utf16(packedContent.AsSpan(offset, lenA));
                offset += lenA;
                var listM = new List<ulong>(lenM);
                for (int i = 0; i < lenM; i++)
                {
                    listM.Add(FromBinary.LittleEndian<UInt64>(packedContent.AsSpan(offset, 8)));
                    offset += 8;
                }
                listchat.Add(new JN_Chat(listM, path, name, bio, chsuid));
            }

            return listchat;
        }


        static public List<JN_Message> HERE_IS_UPDATE_CHAT_HISTORY(Byte[] packedContent)
        {
            int len;
            int offset = 0;
            List<JN_Message> listmsg = new();
            while (true)
            {
                len = FromBinary.LittleEndian<int>(packedContent.AsSpan(offset, 4));                        // длинна конкретно текста

                listmsg.Add(new(
                    new DateTime4b(
                        FromBinary.LittleEndian<uint>(packedContent.AsSpan(offset + 4, 4)),
                        FromBinary.LittleEndian<uint>(packedContent.AsSpan(offset + 4 + 4, 4))),
                    FromBinary.Utf16(packedContent.AsSpan(offset + 4 + 4 + 4, len)),
                    FromBinary.LittleEndian<ulong>(packedContent.AsSpan(offset + 4 + 4 + 4 + len, 8)),
                    FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(offset + 4 + 4 + 4 + len + 8, 4))
                ));
                offset += 8 + 8 + 4 + + 4 + len;
                if (offset >= packedContent.Length) break;
            }
            return listmsg;
        }
        static public ChatHistoryUpdate I_REQUEST_CHAT_HISTORY_UPDATE(Byte[] packedContent)
        {
            //  Должен вернуть UInt32 suid чата, а также UInt32 с какой мессаги обновлять
            return new ChatHistoryUpdate(FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(0, 4)), FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(4, 4)));
        }

        public struct ChatHistoryUpdate(UInt32 fromMessageSuid, UInt32 chatSuid)
        {
            public UInt32 ChatSuid { get; } = chatSuid;
            public UInt32 FromMessageSuid { get; } = fromMessageSuid;
        }
    }
}