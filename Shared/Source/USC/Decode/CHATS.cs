using System;
using System.Collections.Generic;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public JN_Chat[] GET_ACTIVE_CHATS(byte[] packedContent)
        {
            int len;
            int offset = 0;
            List<JN_Chat> listchat = new();
            while (true)
            {
                
            }
        }
        static public JN_Message[] UPDATE_CHAT_HISTORY(byte[] packedContent)
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
            return listmsg.ToArray();
        }
    }
}