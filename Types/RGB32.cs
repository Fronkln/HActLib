using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    public class RGB32
    {
        public uint R;
        public uint G;
        public uint B;
        public RGB32() { }
        public RGB32(uint r, uint g, uint b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static implicit operator System.Drawing.Color(RGB32 col)
        {
            return System.Drawing.Color.FromArgb(255, (byte)col.R, (byte)col.G, (byte)col.B);
        }

        public static implicit operator RGB32(System.Drawing.Color col)
        {
            return new RGB32() { R = col.G, G = col.G, B = col.B };
        }
    }
}
