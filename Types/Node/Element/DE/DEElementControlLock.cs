using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x4B)]
    [ElementID(Game.YK2, 0x4B)]
    [ElementID(Game.JE, 0x4B)]
    [ElementID(Game.YLAD, 0x48)]
    [ElementID(Game.LJ, 0x48)]
    [ElementID(Game.LAD7Gaiden, 0x48)]
    [ElementID(Game.LADIW, 0x48)]
    public class DEElementControlLock : NodeElement
    {
        public uint Unknown = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unknown);
        }
    }
}
