using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{

    internal class OEBranchNode : NodeElement
    {
        public short Unk1;
        public int Unk2;
        public int Unk3;
        public int Unk4;
        public int Unk5;
        public int Unk6;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadInt16();
            reader.ReadBytes(2);

            Unk2 = reader.ReadInt32();
            Unk3 = reader.ReadInt32();
            Unk4 = reader.ReadInt32();
            Unk5 = reader.ReadInt32();
            Unk6 = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unk1);
            writer.WriteTimes(0, 2);

            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            writer.Write(Unk6);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 24;
        }
    }
}
