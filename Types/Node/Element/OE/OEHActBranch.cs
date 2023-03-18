using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Ishin, 38)]
    [ElementID(Game.Y5, 38)]
    [ElementID(Game.Y0, 38)]
    [ElementID(Game.YK1, 38)]
    public class OEHActBranch : NodeElement
    {
        public short Unk1 = 1;
        public byte[] Unknown = new byte[6];
        public int Unk2 = 1;
        public int Unk3 = 3;
        public byte[] Unknown2 = new byte[84];
        public int Unk4 = 1;
        public int Unk5 = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadInt16();
            Unknown = reader.ReadBytes(6);
            Unk2 = reader.ReadInt32();
            Unk3 = reader.ReadInt32();
            Unk4 = reader.ReadInt32();
            Unk5 = reader.ReadInt32();
            Unknown2 = reader.ReadBytes(76);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unk1);
            writer.Write(Unknown);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            writer.Write(Unknown2);
        }
    }
}
