using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 28)]
    [ElementID(Game.Ishin, 28)]
    public class OEBodyflash : NodeElement
    {
        public int Y5_Unknown1;
        public int BoneID;
        public float Intensity;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Y5_Unknown1 = reader.ReadInt32();
            BoneID = reader.ReadInt32();
            Intensity = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Y5_Unknown1);
            writer.Write(BoneID);
            writer.Write(Intensity);
        }
    }
}
