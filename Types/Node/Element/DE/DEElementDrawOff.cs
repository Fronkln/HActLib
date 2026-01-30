using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x4)]
    [ElementID(Game.YK2, 0x4)]
    [ElementID(Game.JE, 0x4)]
    [ElementID(Game.YLAD, 0x4)]
    [ElementID(Game.LJ, 0x4)]
    [ElementID(Game.LAD7Gaiden, 0x4)]
    [ElementID(Game.LADIW, 0x4)]
    [ElementID(Game.LADPYIH, 0x4)]
    [ElementID(Game.YK3, 0x4)]
    public class DEElementDrawOff : NodeElement
    {
        public int Unk1 = 0;
        public int Unk2 = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadInt32();
            Unk2 = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unk1);
            writer.Write(Unk2);
        }
    }
}
