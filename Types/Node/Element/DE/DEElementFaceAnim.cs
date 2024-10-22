using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x57)]
    [ElementID(Game.YK2, 0x57)]
    [ElementID(Game.JE, 0x57)]
    [ElementID(Game.YLAD, 0x54)]
    [ElementID(Game.LJ, 0x54)]
    [ElementID(Game.LAD7Gaiden, 0x54)]
    [ElementID(Game.LADIW, 0x54)]
    [ElementID(Game.LADPYIH, 0x54)]
    public class DEElementFaceAnim : NodeElement
    {
        public uint PatternID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            PatternID = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(PatternID);
        }
    }
}
