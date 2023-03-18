using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 50)]
    [ElementID(Game.Ishin, 49)]
    [ElementID(Game.Y0, 49)]

    public class OEHeat : NodeElement
    {
        public ushort Unknown1;
        public int HeatChange;
        public uint Unknown2;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown1 = reader.ReadUInt16();
            reader.ReadBytes(2);
            HeatChange = reader.ReadInt32();
            Unknown2 = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unknown1);
            writer.WriteTimes(0, 2);
            writer.Write(HeatChange);
            writer.Write(Unknown2);
        }
    }
}
