using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

using AVcontrol;


namespace Shared.Source
{
    public class JN_Message(DateTime4b sentTime, string message, UInt64 authorSUID, UInt32 messageSUID)
    {
        public DateTime4b sentTime = sentTime;
        public string message = message;

        public UInt64 authorSUID = authorSUID;

        public UInt32 messageSUID = messageSUID;

        public JN_MessageState state = JN_MessageState.SENDING;
    }
    public enum JN_MessageState : Byte
    {
        SENDING = 0,
        SEND_TO_SERVER = 1,
        RECEIVED_BY_CLIENT = 2,
        READ_BY_CLIENT = 3
    }



    public class JN_Author(string name, string surname, string bio, UInt64 suid, string avatar)
    {
        public string name    = name;
        public string surname = surname;
        public string bio     = bio;

        public UInt64 suid = suid;
        public string avatar = avatar;



        public string FullName => name + " " + surname;
    }


    public class JN_Chat(List<UInt64> membersSUID, string chatAvatar, UInt64 chatSUID)
    {
        public List<UInt64>  membersSUID = membersSUID;
        public string chatAvatar = chatAvatar;

        public UInt64 chatSUID = chatSUID;
        //public List<JN_ChatTopic> topics = topics;
    }
    public class JN_ChatTopic(string topicAvatar, string topicTitle, Int32 topicID) // значительно позже. . .
    {
        public string topicAvatar = topicAvatar;

        public string topicTitle = topicTitle;
        public Int32  topicID    = topicID;
    }
}
