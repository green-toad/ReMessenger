using System;
using System.Collections.Generic;

using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Decode
    {
        static public UnpackedContent UnPack(Byte[] packedContent)
        {
            UnpackedContent unpacked = new();
            if (packedContent != null && packedContent.Length >= 2)
            {
                try
                {
                    Byte unsanitizedCommand = packedContent[0];
                    unsanitizedCommand &= 0b01111111;

                    MainCommand parsedMainCommand = Enum.IsDefined(typeof(MainCommand), unsanitizedCommand)
                        ? (MainCommand)unsanitizedCommand
                        :  MainCommand.UNKNOWN;
                    unpacked.MainCommand = parsedMainCommand;

                    //  Response handling
                    if (parsedMainCommand == MainCommand.UNKNOWN ||
                        parsedMainCommand == MainCommand.ERROR_UNKNOWN                   ||
                        parsedMainCommand == MainCommand.ERROR_PROBABLY_INTERNET_TROUBLE ||
                        parsedMainCommand == MainCommand.ERROR_YOU_NEED_TO_RECONNECT   ||
                        packedContent.Length < 5) return unpacked;

                    if ((packedContent[0] & 0b10000000) != 0)  //  Is compact packet (CONNECTs USC)
                    {
                        unpacked.PackedContent = packedContent[1..];
                        return unpacked;
                    }


                    UInt64 sessionId   = FromBinary.BigEndian<UInt64>(packedContent.AsSpan(1, 4));
                    unpacked.SessionId = sessionId;
                    if (packedContent.Length < 9) return unpacked;

                    UInt64 forResponseSID   = FromBinary.BigEndian<UInt64>(packedContent.AsSpan(5, 4));
                    unpacked.ForResponseSID = forResponseSID;

                    Byte subCommandsCount = packedContent[9];
                    if (parsedMainCommand == MainCommand.UNKNOWN ||
                        parsedMainCommand == MainCommand.OK ||
                        parsedMainCommand == MainCommand.OK_NOW_SYNCING ||
                        parsedMainCommand == MainCommand.NOT_NOW_PLEASE_WAIT_FOR_SYNC ||
                        packedContent.Length < 6 + subCommandsCount) return unpacked;


                    ReKeyExportType reKeyExportType = ReKeyExportType.NOT_PRESENT;
                    SubCommand[] subCommands = new SubCommand[subCommandsCount];
                    for (var i = 0; i < subCommandsCount; i++)
                    {
                        Byte  unsanitizedSubCommand = packedContent[10 + i];
                        SubCommand parsedSubCommand = Enum.IsDefined(typeof(SubCommand), unsanitizedSubCommand)
                            ? (SubCommand)unsanitizedSubCommand
                            :  SubCommand.UNKNOWN;
                        subCommands[i] = parsedSubCommand;

                        reKeyExportType = parsedSubCommand switch
                        {
                            SubCommand.SERVER_HERE_IS_NEW_EKEY or
                            SubCommand.CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY or
                            SubCommand.CLIENT_HERE_IS_NEW_EKEY
                                => reKeyExportType == ReKeyExportType.NOT_PRESENT
                                ? ReKeyExportType.FULL_REKEY : ReKeyExportType.INVALID,

                            SubCommand.SERVER_HERE_IS_NEW_EKEY_PR_ALPHABET or
                            SubCommand.CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_PR_ALPHABET or
                            SubCommand.CLIENT_HERE_IS_NEW_EKEY_PR_ALPHABET
                                => reKeyExportType == ReKeyExportType.NOT_PRESENT
                                ? ReKeyExportType.PR_ALPHABET : ReKeyExportType.INVALID,

                            SubCommand.SERVER_HERE_IS_NEW_EKEY_EX_ALPHABET or
                            SubCommand.CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_EX_ALPHABET or
                            SubCommand.CLIENT_HERE_IS_NEW_EKEY_EX_ALPHABET
                                => reKeyExportType == ReKeyExportType.NOT_PRESENT
                                ? ReKeyExportType.EX_ALPHABET : ReKeyExportType.INVALID,

                            SubCommand.SERVER_HERE_IS_NEW_EKEY_SHIFTS or
                            SubCommand.CLIENT_FROM_SERVER_HERE_IS_NEW_EKEY_SHIFTS or
                            SubCommand.CLIENT_HERE_IS_NEW_EKEY_SHIFTS
                                => reKeyExportType == ReKeyExportType.NOT_PRESENT
                                ? ReKeyExportType.SHIFTS : ReKeyExportType.INVALID,

                            _ => ReKeyExportType.NOT_PRESENT
                        };
                    }
                    unpacked.SubCommands = subCommands;

                    Int32 dataOffset = 10 + subCommandsCount;
                    if (reKeyExportType != ReKeyExportType.NOT_PRESENT)
                    {
                        switch (reKeyExportType)
                        {
                            case ReKeyExportType.FULL_REKEY:
                            {

                                break;
                            }
                            case ReKeyExportType.PR_ALPHABET:
                            {

                                break;
                            }
                            case ReKeyExportType.EX_ALPHABET:
                            {

                                break;
                            }
                            case ReKeyExportType.SHIFTS:
                            {

                                break;
                            }
                            default: return unpacked;
                        }


                        //  тут будет немного больно и сложно, вобщем пока забей, всё равно экспорт ещё точно будет меняться
                        //  прикол в том, что для кастомизации, может прийти любая часть ключа или весь ключ
                        //  и каждый случай надо обрабатывать отдельно, пока забей
                        //  ....
                        //  а, также dataOffset должно измениться ровно на длину присланного ключа

                        unpacked.ReKeyExport = [];
                    }

                    unpacked.PackedContent = packedContent[dataOffset..];
                }
                finally { }
            }

            //   тебе 100% надо будет тестировать это на баги, удачи лол
            return unpacked;
        }



        public class UnpackedContent
        {
            public UInt64 SessionId = 0;
            public UInt64 ForResponseSID = 0;

            public MainCommand  MainCommand = MainCommand.UNKNOWN;
            public SubCommand[] SubCommands = [];

            public List<Byte> ReKeyExport = [];
            public Byte[]   PackedContent = [];


            public UnpackedContent() { }

            public UnpackedContent(UInt64 sessionId, UInt64 forResponseSID,
                MainCommand mainCommand, SubCommand[] subCommands,
                List<Byte> reKeyExport, Byte[] packedContent)
            {
                SessionId = sessionId;
                ForResponseSID = forResponseSID;

                MainCommand = mainCommand;
                SubCommands = subCommands;

                ReKeyExport = reKeyExport;
                PackedContent = packedContent;
            }
            public UnpackedContent(UnpackedContent copyFrom)
            {
                SessionId = copyFrom.SessionId;
                ForResponseSID = copyFrom.ForResponseSID;

                MainCommand = copyFrom.MainCommand;
                SubCommands = [.. copyFrom.SubCommands];

                ReKeyExport   = [.. copyFrom.ReKeyExport];
                PackedContent = [.. copyFrom.PackedContent];
            }
        }
    }
}