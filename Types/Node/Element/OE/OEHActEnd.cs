using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Ishin, 37)]
    [ElementID(Game.Y5, 37)]
    [ElementID(Game.Y0, 37)]
    [ElementID(Game.YK1, 37)]
    [ElementID(Game.FOTNS, 38)]

    public class OEHActEnd : NodeElement
    {
        public int Unknown = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {   
            Unknown = reader.ReadInt32();
            reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unknown);
            writer.WriteTimes(0, 12);
        }
    }
}
