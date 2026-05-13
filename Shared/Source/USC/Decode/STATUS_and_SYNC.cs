using System;
using System.Linq;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public MessageStatus MESSAGE_STATUS(Byte[] packedContent)
        {
            Byte unsanitizedMessageStatus = packedContent.FirstOrDefault();
            return Enum.IsDefined(typeof(MessageStatus), unsanitizedMessageStatus)
                            ? (MessageStatus)unsanitizedMessageStatus
                            : MessageStatus.UNKNOWN;
        }
        static public PingStatus PING_STATUS(Byte[] packedContent)
        {
            Byte unsanitizedPingStatus = packedContent.FirstOrDefault();
            return Enum.IsDefined(typeof(PingStatus), unsanitizedPingStatus)
                            ? (PingStatus)unsanitizedPingStatus
                            : PingStatus.UNKNOWN;
        }


        static public Byte TRY_CHANGE_HEAD_DEVICE(Byte[] packedContent)
            => packedContent.FirstOrDefault();
    }
}