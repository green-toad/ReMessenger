using System.ComponentModel.DataAnnotations;
using AVcontrol;
using Microsoft.EntityFrameworkCore;
using Shared.Source;



namespace MessengerServer
{
    // jabaNetPassword

    /*
    * File.db
    *      Users                           (table) 
    *          SUID        (UInt64)[key]
    *          Name        (VRCHAR(40))
    *          Surname     (VRCHAR(40))
    *          Bio         (VRCHAR(120))
    *          Avatar      (VRCHAR(150))       это путь до файла
    *
    *      UserDevices                     (table)
    *          userSUID    (UInt64)[key]
    *          deviceID    (UInt16)
    *          sessionID   (UInt64)
    *          isSynced    (Bool)
    *
    *      Messages                        (table)
    *          SUID        (UInt64)[key]
    *          Time        (TIME)
    *          Owner       (UInt64)            это SUID отправителя
    *          Membership  (UInt64)            это SUID чата, в котором лежит месага
    *          TypeMessage (enum file/pic/music/. . .)
    *          Content     (nullable TEXT)      путь до файла/текст сообщения (зависит от типа)
    *      
    *      Chats                           (table)
    *          SUID        (UInt64)[key]
    *          Name        (VRCHAR(40))
    *          Bio         (VRCHAR(120))
    *          Avatar      (VRCHAR(150))
    *      
    *      Participants                    (table)
    *          rID         (primary key)
    *          UserSUID    (UInt64)
    *          ChatSUID    (UInt64)
    *          UserRole    (Uint8)            банальным enum мы обозначим роли как цифры (вледлец-0, админ-1, участник-2, читатель-3) - что то вроде такого
    */

    public class User : JN_Author
    {
        public User(string name, string surname, string bio, ulong suid, string avatar) : base(name, surname, bio, suid, avatar) { }

        [Key]
        public UInt64 SUID { get => suid; set => suid = value; }
        [MaxLength(40)]
        public string Name { get => name ; set => name = value ; }
        [MaxLength(40)]
        public string Surname { get => surname ; set => surname = value ; }
        [MaxLength(120)]
        public string Bio { get => bio; set => bio = value; }
        [MaxLength(150)]
        public string Avatar { get => avatar; set => avatar = value; }
    }

    [PrimaryKey(nameof(UserSUID), nameof(deviceID), nameof(sessionID))]
    public class UserDevices
    {
        public UInt64 UserSUID { get; set; }
        public byte deviceID { get; set; }
        public UInt64 sessionID { get; set; }
        public bool isSynced { get; set; }
    }

    public class Message : JN_Message
    {
        public Message(DateTime4b sentTime, string message, ulong authorSUID, uint messageSUID) : base(sentTime, message, authorSUID, messageSUID) { }

        [Key]
        public UInt32 SUID { get => messageSUID; set => messageSUID = value; }
        public DateTime Time { get => sentTime.; set => sentTime = new DateTime4b(value); } // докопаться до егора
        public UInt64 Owner { get => authorSUID; set => authorSUID = value; }
        public UInt64 Membership { get; set; }
        public Type ContentType { get; set; }
        public string? Content { get => message; set => message = value; }

        public enum Type
        {
            text,
            file,           // потом добавим войс
        }
    }

    public class Chat : JN_Chat
    {
        public Chat(List<ulong> membersSUID, string chatAvatar, string name, string bio, ulong chatSUID) : base(membersSUID, chatAvatar, name, bio, chatSUID) { }

        [Key]
        public UInt64 SUID { get => chatSUID; set => chatSUID = value; }
        public UInt64 OwnerSUID { get; set; }
        [MaxLength(40)]
        public string Name { get => name; set => name = value; }
        [MaxLength(120)]
        public string Bio { get => bio; set => bio = value; }
        [MaxLength(150)]
        public string Avatar { get => chatAvatar; set => chatAvatar = value; }
        public ICollection<Participant> Participants { get; set; } 
    }

    [PrimaryKey(nameof(UserSUID), nameof(ChatSUID))]
    public class Participant
    {
        public UInt64 UserSUID { get; set; }
        public UInt64 ChatSUID { get; set; }
        public Chat ChatLink { get; set; } 
        public Type UserRole { get; set; }

        public enum Type
        {
            admin,
            participant,
            observer,
        }
    }
}
