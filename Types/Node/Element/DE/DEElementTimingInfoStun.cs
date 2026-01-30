using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x62)]
    [ElementID(Game.YK2, 0x62)]
    [ElementID(Game.JE, 0x62)]
    [ElementID(Game.YLAD, 0x5F)]
    [ElementID(Game.LJ, 0x5F)]
    [ElementID(Game.LAD7Gaiden, 0x5F)]
    [ElementID(Game.LADIW, 0x5F)]
    [ElementID(Game.LADPYIH, 0x5F)]
    [ElementID(Game.YK3, 0x5F)]
    public class DEElementTimingInfoStun : NodeElement
    {
        public uint Type;
        public uint GmtID;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Type = reader.ReadUInt32();
            GmtID = reader.ReadUInt32();

            reader.ReadBytes(8);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Type);
            writer.Write(GmtID);

            writer.WriteTimes(0, 8);
        }
    }
}
