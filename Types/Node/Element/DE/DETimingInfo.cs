using System;
using Yarhl.IO;

namespace HActLib
{
    public class DETimingInfo : NodeElement
    {
        public uint Param;
        public uint BattleBits;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Param = reader.ReadUInt32();
            BattleBits = reader.ReadUInt32();

            reader.ReadBytes(8);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Param);
            writer.Write(BattleBits);

            writer.WriteTimes(0, 8);
        }
    }
}
