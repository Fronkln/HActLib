using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xA2)]
    [ElementID(Game.YK2, 0xA2)]
    [ElementID(Game.JE, 0xA2)]
    [ElementID(Game.YLAD, 0x9E)]
    [ElementID(Game.LJ, 0x9E)]
    [ElementID(Game.LAD7Gaiden, 0x9E)]
    [ElementID(Game.LADIW, 0x9E)]
    [ElementID(Game.LADPYIH, 0x9E)]
    public class DEElementCharacterNodeScale : NodeElement
    {
        public float HeadScale = 1;
        public float HandScale = 1;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            HeadScale = reader.ReadSingle();
            HandScale = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(HeadScale);
            writer.Write(HandScale);
        }
    }
}
