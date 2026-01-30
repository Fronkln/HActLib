using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xBC)]
    [ElementID(Game.YK2, 0xBC)]
    [ElementID(Game.JE, 0xBC)]
    [ElementID(Game.YLAD, 0xB8)]
    [ElementID(Game.LJ, 0xB8)]
    [ElementID(Game.LAD7Gaiden, 0xB8)]
    [ElementID(Game.LADIW, 0xB8)]
    [ElementID(Game.LADPYIH, 0xB8)]
    [ElementID(Game.YK3, 0xB8)]
    public class DEHActInputBarrage : NodeElement
    {
        public uint InputID;
        public uint CondFlagNo;
        public uint BarrageCount;
        public uint DecideTick;
       // public uint PlayLimitCT;

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 14;
        }

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            InputID = reader.ReadUInt32();
            CondFlagNo = reader.ReadUInt32();
            BarrageCount = reader.ReadUInt32();
            DecideTick = reader.ReadUInt32();

            reader.ReadBytes(8);

         //   reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(InputID);
            writer.Write(CondFlagNo);
            writer.Write(BarrageCount);
            writer.Write(DecideTick);
           
            writer.WriteTimes(0, 8);
        }

    }
}
