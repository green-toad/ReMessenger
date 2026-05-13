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
            if (packedContent != null && packedContent.Length >= 6)
            {
                try
                {
                    UInt64 sessionId   = FromBinary.BigEndian<UInt64>(packedContent.AsSpan(0, 4));
                    unpacked.SessionId = sessionId;

                    Byte unsanitizedCommand = packedContent[5];
                    MainCommand parsedMainCommand = Enum.IsDefined(typeof(MainCommand), unsanitizedCommand)
                        ? (MainCommand)unsanitizedCommand
                        :  MainCommand.UNKNOWN;
                    unpacked.MainCommand = parsedMainCommand;

                    Byte subCommandsCount = packedContent[6];

                    if (parsedMainCommand == MainCommand.UNKNOWN ||
                        packedContent.Length < 6 + subCommandsCount) return unpacked;

                    ReKeyExportType reKeyExportType = ReKeyExportType.NOT_PRESENT;
                    SubCommand[] subCommands = new SubCommand[subCommandsCount];
                    for (var i = 0; i < subCommandsCount; i++)
                    {
                        Byte  unsanitizedSubCommand = packedContent[7 + i];
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

                    Int32 dataOffset = 7 + subCommandsCount;
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

            public MainCommand  MainCommand = MainCommand.UNKNOWN;
            public SubCommand[] SubCommands = [];

            public List<Byte> ReKeyExport = [];
            public Byte[]   PackedContent = [];


            public UnpackedContent() { }

            public UnpackedContent(UInt64 sessionId,
                MainCommand mainCommand, SubCommand[] subCommands,
                List<Byte> reKeyExport, Byte[] packedContent)
            {
                SessionId = sessionId;

                MainCommand = mainCommand;
                SubCommands = subCommands;

                ReKeyExport = reKeyExport;
                PackedContent = packedContent;
            }
            public UnpackedContent(UnpackedContent copyFrom)
            {
                SessionId = copyFrom.SessionId;

                MainCommand = copyFrom.MainCommand;
                SubCommands = [.. copyFrom.SubCommands];

                ReKeyExport   = [.. copyFrom.ReKeyExport];
                PackedContent = [.. copyFrom.PackedContent];
            }
        }
    }
}