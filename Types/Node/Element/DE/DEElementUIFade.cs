using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x33)]
    [ElementID(Game.YK2, 0x33)]
    [ElementID(Game.JE, 0x33)]
    [ElementID(Game.YLAD, 0x31)]
    [ElementID(Game.LJ, 0x31)]
    [ElementID(Game.LAD7Gaiden, 0x31)]
    [ElementID(Game.LADIW, 0x31)]
    public class DEElementUIFade : NodeElement
    {
        public uint Flags = 1;
        public uint InFrame = 0;
        public uint OutFrame = 0;

        public float[] Animation = new float[32];
        public RGBA32 Color = new RGBA32(255, 255, 255, 255);

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Flags = reader.ReadUInt32();
            InFrame = reader.ReadUInt32();
            OutFrame = reader.ReadUInt32();

            reader.ReadBytes(4);

            for (int i = 0; i < 32; i++)
                Animation[i] = reader.ReadSingle();

            Color = (RGBA32)ConvertFormat.With<RGBA32Convert>(new ConversionInf(new BinaryFormat(reader.Stream), reader.Endianness));
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Flags);
            writer.Write(InFrame);
            writer.Write(OutFrame);

            writer.WriteTimes(0, 4);

            for (int i = 0; i < 32; i++)
                writer.Write(Animation[i]);

            writer.Write(Color.R);
            writer.Write(Color.G);
            writer.Write(Color.B);
            writer.Write(Color.A);
        }
    }
}
