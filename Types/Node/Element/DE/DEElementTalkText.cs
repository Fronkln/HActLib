using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xFB)]
    [ElementID(Game.YK2, 0xFB)]
    [ElementID(Game.JE, 0xFB)]
    [ElementID(Game.YLAD, 0xF7)]
    [ElementID(Game.LJ, 0xF7)]
    [ElementID(Game.LAD7Gaiden, 0xF7)]
    [ElementID(Game.LADIW, 0xF7)]
    [ElementID(Game.LADPYIH, 0xF7)]
    public class DEElementTalkText : NodeElement
    {
        public uint TalkCategory;
        public uint TextID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            TalkCategory = reader.ReadUInt32();
            TextID = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(TalkCategory);
            writer.Write(TextID);
        }
    }
}
