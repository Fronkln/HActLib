using System;
using System.Drawing;
using System.Net;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 9)]
    [ElementID(Game.Ishin, 9)]
    //[ElementID(Game.Y0, 9)]
    public class OEGradation : OEBaseEffect
    {
        public uint Unknown1;
        public uint Unknown2;

        public RGB32 Color1;
        public uint Unknown3;
        public RGB32 Color2;
        public float Rotation;
        public float Color1Width;
        public float Color2Width;

        public RGBA32 UnknownColor1;
        public RGBA32 UnknownColor2;

        public float Unknown4;
        public float Unknown5;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
            Color1 = reader.ReadRGB32();
            Unknown3 = reader.ReadUInt32();
            Color2 = reader.ReadRGB32();
            Rotation = reader.ReadSingle();
            Color1Width = reader.ReadSingle();
            Color2Width = reader.ReadSingle();
            UnknownColor1 = reader.ReadRGBA32();
            UnknownColor2 = reader.ReadRGBA32();
            Unknown4 = reader.ReadSingle();
            Unknown5 = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Color1);
            writer.Write(Unknown3);
            writer.Write(Color2);
            writer.Write(Rotation);
            writer.Write(Color1Width);
            writer.Write(Color2Width);
            writer.Write(UnknownColor1);
            writer.Write(UnknownColor2);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
        }
    }
}
