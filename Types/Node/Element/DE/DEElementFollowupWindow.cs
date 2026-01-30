using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x4A)]
    [ElementID(Game.YK2, 0x4A)]
    [ElementID(Game.JE, 0x4A)]
    [ElementID(Game.YLAD, 0x47)]
    [ElementID(Game.LJ, 0x47)]
    [ElementID(Game.LAD7Gaiden, 0x47)]
    [ElementID(Game.LADIW, 0x47)]
    [ElementID(Game.LADPYIH, 0x47)]
    [ElementID(Game.YK3, 0x47)]
    public class DEElementFollowupWindow : NodeElement
    {
        public uint Unknown = 0;
        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unknown);
        }
    }
}
