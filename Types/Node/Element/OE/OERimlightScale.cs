using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y0, 67)]
    public class OERimlightScale : NodeElement
    {
        public float Scale;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Scale = reader.ReadSingle();
            reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Scale);
            writer.WriteTimes(0, 12);
        }
    }
}
