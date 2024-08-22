using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.LJ, 0x1C5)]
    [ElementID(Game.LAD7Gaiden, 0x1C5)]
    [ElementID(Game.LADIW, 0x1C5)]
    public class DEElementFullscreenAuthMovie : NodeElement
    {
        public string Movie;
        public uint MovieStart;
        public uint MovieEnd;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Movie = reader.ReadString(64);
            MovieStart = reader.ReadUInt32();
            MovieEnd = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Movie.ToLength(64));
            writer.Write(MovieStart);
            writer.Write(MovieEnd);
        }
    }
}
