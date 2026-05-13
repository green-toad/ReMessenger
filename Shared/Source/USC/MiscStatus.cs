using System;



namespace Shared.Source.USC
{
    public enum PingStatus
    {
        I_AM_CHECKING = 0,
        I_AM_ONLINE = 1,
        I_AM_OFFLINE = 2,

        UNKNOWN = 255
    }

    public enum MessageStatus
    {
        SENDING = 0,
        RECEIVED_BY_SERVER = 1,
        READ_BY_USER = 2,

        UNKNOWN = 255
    }

    public enum ReKeyExportType
    {
        FULL_REKEY  = 0,
        PR_ALPHABET = 1,
        EX_ALPHABET = 2,
        SHIFTS = 3,

        NOT_PRESENT = 4,
        INVALID = 5
    }
}