using System;
using System.Linq;
using System.Collections.Generic;

using AVcontrol;



namespace Shared.Source.USC
{
    static public partial class Encode
    {
        static public Byte[] PackTogether(
            MainCommand mainCommand,
            UInt64 sessionId, UInt64 forResponseSID,
            SubCommand[] subCommands,
            Byte[] packedContent)
        {
            return
            [
                (Byte)(mainCommand + 128),  //  +128 means 1st BIT indicates that sessionIds will be passed
                .. ToBinary.BigEndian(sessionId),
                .. ToBinary.BigEndian(forResponseSID),
                (Byte)subCommands.Length,
                .. subCommands.Select(e => (Byte)e).ToArray(),
                .. packedContent
            ];
        }

        static public Byte[] PackTogether(
            MainCommand mainCommand,
            UInt64 sessionId, UInt64 forResponseSID,
            SubCommand[] subCommands,
            List<Byte> reKeyExport, Byte[] packedContent)
        {
            return
            [
                (Byte)(mainCommand + 128),  //  +128 means 1st BIT indicates that sessionIds will be passed
                .. ToBinary.BigEndian(sessionId),
                .. ToBinary.BigEndian(forResponseSID),
                (Byte)subCommands.Length,
                .. subCommands.Select(cmd => (Byte)cmd).ToArray(),
                .. reKeyExport,
                .. packedContent
            ];
        }



        static public Byte[] PackResponse(MainCommand responseCode, UInt64 responseSID, UInt64 newSessionId)
        {
            return
            [
                (Byte)responseCode,
                .. ToBinary.BigEndian(responseSID),
                .. ToBinary.BigEndian(newSessionId)
            ];
        }
    }
}