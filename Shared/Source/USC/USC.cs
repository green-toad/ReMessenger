using System;



namespace Shared.Source.USC
{
    public enum MainCommand : Byte
    {
        CONNECT_CLIENT_SERVER_1_EN = 0,
        CONNECT_CLIENT_SERVER_2_EE = 1,
        CONNECT_CLIENT_SERVER_3_NE = 2,

        CONNECT_CLIENT_CLIENT_1_EN = 3,
        CONNECT_CLIENT_CLIENT_2_EE = 4,
        CONNECT_CLIENT_CLIENT_3_NE = 5,

        STD_AUTHENTICATION   = 6,
        TOKEN_AUTHENTICATION = 7,

        HERE_IS_SYNC    = 8,                    // ответ сервера
        I_RECEIVED_SYNC = 9,                    // запрос клиента
        TRY_CHANGE_MY_DEVICE_PRIORITY = 10,

        I_REQUEST_ACTIVE_CHATS = 11,            // запрос клиента
        HERE_IS_ACTIVE_CHATS   = 12,            // ответ сервера
        I_REQUEST_CHAT_HISTORY_UPDATE = 13,     // запрос клиента
        HERE_IS_CHAT_HISTORY_UPDATE   = 14,     // ответ сервера

        UPDATE_PING_STATUS    = 15,
        UPDATE_MESSAGE_STATUS = 16,

        SEND_MSG   = 17,                        // запрос клиента
        SEND_PIC   = 18,                        // запрос клиента
        SEND_FILE  = 19,                        // запрос клиента
        SEND_MUSIC = 20,                        // запрос клиента
        DELETE_MSG = 21,                        // запрс клиента

        CHANGE_LOGIN    = 22,
        CHANGE_PASSWORD = 23,
        DELETE_ACCOUNT  = 24,

        //-----  RESPONSES
        OK = 249,
        OK_NOW_SYNCING = 250,
        NOT_NOW_PLEASE_WAIT_FOR_SYNC = 251,

        ERROR_UNKNOWN = 252,
        ERROR_PROBABLY_INTERNET_TROUBLE = 253,
        ERROR_YOU_NEED_TO_REAUTHORISE = 254,

        //-----  MISC
        UNKNOWN = 255
    }



    public enum SubCommand : Byte
    {
        NONE = 0,

        SERVER_HERE_IS_NEW_EKEY = 1,
        SERVER_HERE_IS_NEW_EKEY_PR_ALPHABET = 2,
        SERVER_HERE_IS_NEW_EKEY_EX_ALPHABET = 3,
        SERVER_HERE_IS_NEW_EKEY_SHIFTS = 4,

        CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY = 5,
        CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_PR_ALPHABET = 6,
        CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_EX_ALPHABET = 7,
        CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_SHIFTS = 8,

        CLIENT_HERE_IS_NEW_EKEY = 9,
        CLIENT_HERE_IS_NEW_EKEY_PR_ALPHABET = 10,
        CLIENT_HERE_IS_NEW_EKEY_EX_ALPHABET = 11,
        CLIENT_HERE_IS_NEW_EKEY_SHIFTS = 12,

        SWITCH_MY_SESSION_ID_TO_NEW_AND_SEND_IT_BACK = 13,
        HERE_IS_YOUR_NEW_SESSION_ID = 14,

        UNKNOWN = 255
    }
}