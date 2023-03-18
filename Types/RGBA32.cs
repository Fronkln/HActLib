using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    public class RGBA32
    {
        public uint R;
        public uint G;
        public uint B;
        public uint A;
        public RGBA32() { }
        public RGBA32(uint r, uint g, uint b, uint a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static implicit operator System.Drawing.Color(RGBA32 col)
        {
            return System.Drawing.Color.FromArgb((byte)col.R, (byte)col.G, (byte)col.B, (byte)col.A);
        }

        public static implicit operator RGBA32(System.Drawing.Color col)
        {
            return new RGBA32() { R = col.G, G = col.G, B = col.B, A = col.A };
        }
    }

    public class RGBA32Convert : IConverter<ConversionInf, RGBA32>
    {
        public RGBA32 Convert(ConversionInf conv)
        {
            DataReader reader = new DataReader(conv.format.Stream) { Endianness = conv.endianness, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };
            RGBA32 rgba = new RGBA32();

            rgba.R = reader.ReadUInt32();
            rgba.G = reader.ReadUInt32();
            rgba.B = reader.ReadUInt32();
            rgba.A = reader.ReadUInt32();

            return rgba;
        }
    }
}
