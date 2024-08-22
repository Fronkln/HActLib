using System;
using Yarhl.IO;

namespace HActLib
{

    [ElementID(Game.Y5, 73)]
    [ElementID(Game.Ishin, 73)]
    [ElementID(Game.Y0, 71)]
    [ElementID(Game.YK1, 71)]
    public class OEHActStopEnd : NodeElement
    {
        public short Unknown = 1;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown = reader.ReadInt16();
            reader.ReadBytes(2);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unknown);
            writer.WriteTimes(0, 2);
        }
    }
}
