using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xC1)]
    [ElementID(Game.YK2, 0xC1)]
    [ElementID(Game.JE, 0xC1)]
    [ElementID(Game.YLAD, 0xBD)]
    [ElementID(Game.LJ, 0xBD)]
    [ElementID(Game.Gaiden, 0xBD)]
    [ElementID(Game.Y8, 0xBD)]
    public class DEElementStageWarp : NodeElement
    {
        public uint WarpId;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            WarpId = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(WarpId);
        }
    }
}
