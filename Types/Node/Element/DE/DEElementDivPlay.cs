using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x64)]
    [ElementID(Game.YK2, 0x64)]
    [ElementID(Game.JE, 0x64)]
    [ElementID(Game.YLAD, 0x61)]
    [ElementID(Game.LJ, 0x61)]
    [ElementID(Game.LAD7Gaiden, 0x61)]
    [ElementID(Game.LADIW, 0x61)]
    public  class DEElementDivPlay : NodeElement
    {
        public string FileName;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            FileName = reader.ReadString(128);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(FileName.ToLength(128), maxSize: 128);
        }
    }
}
