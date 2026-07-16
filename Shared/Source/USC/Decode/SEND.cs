using System;
using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public JN_Message SEND_MSG(Byte[] packedContent)
        {
            var len = FromBinary.LittleEndian<Int32>(packedContent.AsSpan(0, 4));

            return new JN_Message(
                new DateTime4b(
                    FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(4, 4)),
                    FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(8, 4))),
                FromBinary.Utf16(packedContent.AsSpan(12, len)),
                FromBinary.LittleEndian<UInt64>(packedContent.AsSpan(12 + len, 8)),
                FromBinary.LittleEndian<UInt64>(packedContent.AsSpan(12 + len + 8, 8)),
                FromBinary.LittleEndian<UInt32>(packedContent.AsSpan(12 + len + 16, 4))
            );
        }
        static public JN_Message SEND_PIC(Byte[] packedContent)                 //в текст сообщения суется строка с именем картинки. сама картинка летит отдельно через пересылку файлов.
        {
            return SEND_MSG(packedContent);
        }
        static public JN_Message SEND_FILE(Byte[] packedContent)                //аналагично картинке
        {
            return SEND_MSG(packedContent);
        }
        static public JN_Message SEND_MUSIC(Byte[] packedContent)               //аналогично картинке
        {
            return SEND_MSG(packedContent);
        }
        static public UInt32 DELETE_MSG(Byte[] packedContent)                   //айди сообщения, которое надо удалить
        {
            return FromBinary.LittleEndian<UInt32>(packedContent);
        }
    }
}