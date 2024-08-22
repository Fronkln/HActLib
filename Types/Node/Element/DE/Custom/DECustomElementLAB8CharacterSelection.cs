using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.LADIW, 70010)]
    public class DECustomElementLAB8CharacterSelection : CustomDENodeAuthExtendedElement
    {
        public uint PlayerID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            PlayerID = reader.ReadUInt32();
            reader.Stream.Position += 12;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);
        
            writer.Write(PlayerID);
            writer.WriteTimes(0, 12);
        }
    }
}
