using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 6)]
    [ElementID(Game.Ishin, 6)]
    [ElementID(Game.Y0, 6)]
    public class OEFade : OEBaseEffect
    {
        public RGBA32 Color = new RGBA32();
        public byte[] Animation = new byte[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            Color = (RGBA32)ConvertFormat.With<RGBA32Convert>(new ConversionInf(new BinaryFormat(reader.Stream), reader.Endianness));
            Animation = reader.ReadBytes(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            base.WriteElementData(writer, version);

            writer.Write(Color.R);
            writer.Write(Color.G);
            writer.Write(Color.B);
            writer.Write(Color.A);

            writer.Write(Animation);
        }
    }
}
