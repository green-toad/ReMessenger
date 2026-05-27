using AVcontrol;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] GET_ACTIVE_CHATS(SubCommand[] subCommands)
        {
            throw new NotImplementedException();
        }
        static public byte[] UPDATE_CHAT_HISTORY(JN_Message[] chatStory)
        {
            int totalLength = 0;
            foreach (var msg in chatStory)
            {
                totalLength += 4 + 8 + Encoding.Unicode.GetByteCount(msg.message) + 8 + 4;
            }
            byte[] result = new byte[totalLength];
            int offset = 0;

            foreach (var msg in chatStory)
            {
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
                offset += 4;
            }

            return result;
        }
    }
}