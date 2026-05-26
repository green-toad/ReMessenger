using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] GET_ACTIVE_CHATS(SubCommand[] subCommands)
        {
            throw new NotImplementedException();
        }
        static public Byte[] UPDATE_CHAT_HISTORY(JN_Message[] chatStory, SubCommand[]? subCommands=null)
        {
            List<byte> res = new List<byte>();

            foreach (var msg in chatStory)
            {
                var text = ToBinary.Utf16(msg.message);
                uint len = (uint)text.Length;
            }
        }
    }
}