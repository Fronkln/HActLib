using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x9E)]
    [ElementID(Game.YK2, 0x9E)]
    [ElementID(Game.JE, 0x9E)]
    [ElementID(Game.YLAD, 0x9A)]
    [ElementID(Game.LJ, 0x9A)]
    [ElementID(Game.LAD7Gaiden, 0x9A)]
    [ElementID(Game.LADIW, 0x9A)]
    [ElementID(Game.LADPYIH, 0x9A)]
    public class DEElementReduceAssetUse : NodeElement
    {
        public enum EFlag
        {
            ForceBreak = 0,
            DisableBreak = 1,
            ForceDelete = 2,
            Select = 3,
            PlayOneshot = 4,
        }

        public int ReduceCount = 10;
        public EFlag Flags = 0; //end of v0

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint elementVersion = reader.ReadUInt32();

            ReduceCount = reader.ReadInt32();
            Flags = (EFlag)reader.ReadUInt32();

            if (elementVersion > 0)
                reader.Stream.Position += 4;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(version > GameVersion.Yakuza6 ? 1 : 0);

            writer.Write(ReduceCount);
            writer.Write((uint)Flags);

            if (version > GameVersion.Yakuza6)
                writer.WriteTimes(0, 4);
        }
    }
}
