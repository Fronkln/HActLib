using PIBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 28)]
   // [ElementID(Game.Ishin, 28)]
    public class OEBodyflash : NodeElement
    {
        public int Y5_Unknown1;
        public int BoneID;
        public float Intensity;
        public float Y5_Unknown2;
        public RGB Color;
        public float Intensity2;
        public float Y5_Unknown3;
        public RGB Color2;
        public float Y5_Unknown4;
        public float Y5_Unknown5;
        public int BoneID2;

        public byte[] UnkDat = new byte[28];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Y5_Unknown1 = reader.ReadInt32();
            BoneID = reader.ReadInt32();
            Intensity = reader.ReadSingle();
            Y5_Unknown2 = reader.ReadSingle();
            Color = reader.ReadRGB();
            Intensity2 = reader.ReadSingle();
            Y5_Unknown3 = reader.ReadSingle();
            Color2 = reader.ReadRGB();
            Y5_Unknown4 = reader.ReadSingle();
            Y5_Unknown5 = reader.ReadSingle();
            BoneID2 = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Y5_Unknown1);
            writer.Write(BoneID);
            writer.Write(Intensity);
            writer.Write(Y5_Unknown2);
            writer.Write(Color);
            writer.Write(Intensity2);
            writer.Write(Y5_Unknown3);
            writer.Write(Color2);
            writer.Write(Y5_Unknown4);
            writer.Write(Y5_Unknown5);
            writer.Write(BoneID2);
        }
    }
}
