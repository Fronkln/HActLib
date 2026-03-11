using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xBB)]
    [ElementID(Game.YK2, 0xBB)]
    [ElementID(Game.JE, 0xBB)]
    [ElementID(Game.YLAD, 0xB7)]
    [ElementID(Game.LJ, 0xB7)]
    [ElementID(Game.LAD7Gaiden, 0xB7)]
    [ElementID(Game.LADIW, 0xB7)]
    [ElementID(Game.LADPYIH, 0xB7)]
    [ElementID(Game.YK3, 0xB7)]
    public class DEHActInput : NodeElement
    {
        public uint InputID;
        public uint CondFlagNo;
        public uint DecideTick;
        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return (20 + 28) / 4;
        }

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            //check ver between 1.0 and 2.0
            InputID = reader.ReadUInt32();
            CondFlagNo = reader.ReadUInt32();

            if (version < GameVersion.DE3)
                DecideTick = reader.ReadUInt32();
            else
                DecideTick = new GameTick2(reader.ReadUInt32()).ClassicTick;
            
            reader.ReadBytes(4);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(InputID);
            writer.Write(CondFlagNo);

            if (version < GameVersion.DE3)
                writer.Write(DecideTick);
            else
                writer.Write(new GameTick(DecideTick).NewTick.Tick);

            writer.WriteTimes(0, 4);
        }

    }
}
