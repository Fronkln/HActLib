using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x95)]
    [ElementID(Game.YK2, 0x95)]
    [ElementID(Game.JE, 0x95)]
    [ElementID(Game.YLAD, 0x91)]
    [ElementID(Game.LJ, 0x91)]
    [ElementID(Game.LAD7Gaiden, 0x91)]
    [ElementID(Game.LADIW, 0x91)]
    public class DEElementHideAsset : NodeElement
    {
        public AttachmentSlot ID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {;
            ID = (AttachmentSlot)reader.ReadUInt32();
            reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write((uint)ID);
            writer.WriteTimes(0, 12);
        }
    }
}
