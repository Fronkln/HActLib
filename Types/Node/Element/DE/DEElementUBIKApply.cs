using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using Yarhl.IO;

namespace HActLib
{

    [ElementID(Game.LAD7Gaiden, 0x1E6)]
    [ElementID(Game.LADIW, 0x1E6)]
    [ElementID(Game.LADPYIH, 0x1E6)]
    public class DEElementUBIKApply : NodeElement
    {
        public int Unknown1;
        public int Unknown2;
        public string Ubik;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Ubik = reader.ReadString(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Ubik.ToLength(32));
        }
    }
}
