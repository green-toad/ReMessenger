using System;



namespace Shared.Source.USC
{
    public enum MainCommand : Byte
    {
        CONNECT_CLIENT_SERVER_1_EN = 0,         // клиент -> серверу
        CONNECT_CLIENT_SERVER_2_EE = 1,         // сервер -> клиенту
        CONNECT_CLIENT_SERVER_3_NE = 2,         // клиент -> серверу

        CONNECT_CLIENT_CLIENT_1_EN = 3,         // клиент1 -> сервер -> клиенту2
        CONNECT_CLIENT_CLIENT_2_EE = 4,         // клиент2 -> сервер -> клиенту1
        CONNECT_CLIENT_CLIENT_3_NE = 5,         // клиент1 -> сервер -> клиенту2

        STD_AUTHENTICATION   = 6,               // авторизация через пароль
        TOKEN_AUTHENTICATION = 7,               // автоматической авторизация через авторизационный токен
        HERE_IS_TOKEN_FOR_NEXT_AUTH = 8,        // токен от сервера клиенту для следующей автоматической авторизации

        HERE_IS_SYNC    = 9,                    // ответ сервера
        I_RECEIVED_SYNC = 10,                   // запрос клиента
        TRY_CHANGE_MY_DEVICE_PRIORITY = 11,

        I_REQUEST_ACTIVE_CHATS = 12,            // запрос клиента
        HERE_IS_ACTIVE_CHATS   = 13,            // ответ сервера
        I_REQUEST_CHAT_HISTORY_UPDATE = 14,     // запрос клиента
        HERE_IS_CHAT_HISTORY_UPDATE   = 15,     // ответ сервера

        UPDATE_PING_STATUS    = 16,
        UPDATE_MESSAGE_STATUS = 17,

        SEND_MSG   = 18,                        // запрос клиента
        SEND_PIC   = 19,                        // запрос клиента
        SEND_FILE  = 20,                        // запрос клиента
        SEND_MUSIC = 21,                        // запрос клиента
        DELETE_MSG = 22,                        // запрс клиента

        CHANGE_PASSWORD = 23,
        DELETE_ACCOUNT  = 24,

        CREATE_GROUP = 25,
        ADD_TO_GROUP = 26,
        REMOVE_FROM_GROUP = 27,
        DELETE_GROUP = 28,

        //-----  RESPONSES
        OK = 119,
        OK_NOW_SYNCING = 120,
        NOT_NOW_PLEASE_WAIT_FOR_SYNC = 121,

        ERROR_UNKNOWN = 122,
        ERROR_WRONG_PASSWORD = 123,
        ERROR_INCORRECT_DATA = 124,

        ERROR_PROBABLY_INTERNET_TROUBLE = 125,
        ERROR_YOU_NEED_TO_RECONNECT = 126,

        //-----  MISC
        UNKNOWN = 127
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