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

        public RGB32 Clamp()
        {
            RGB32 rgb = new RGB32(R, G, B);

            if (rgb.R < 0)
                rgb.R = 0;
            if(rgb.R > 255)
                rgb.R = 255;

            if (rgb.G < 0)
                rgb.G = 0;
            if (rgb.G > 255)
                rgb.G = 255;

            if (rgb.B < 0)
                rgb.B = 0;
            if (rgb.B > 255)
                rgb.B = 255;

            return rgb;
        }
    }
}
