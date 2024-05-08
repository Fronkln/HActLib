using System.Data.Common;
using Yarhl.IO.Serialization.Attributes;
using System;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RGB
    {
        public float R { get; set; } = 0;
        public float G { get; set; } = 0;
        public float B { get; set; } = 0;

        public static implicit operator System.Drawing.Color(RGB col)
        {
            return System.Drawing.Color.FromArgb((int)(Math.Abs(col.R * 255)), (int)(Math.Abs(col.G * 255)), (int)(Math.Abs(col.B * 255)));
        }

        public static implicit operator RGB(System.Drawing.Color col)
        {
            return new RGB() { R = col.R / 255f, G = col.G / 255f, B = col.B / 255f };
        }

        public RGB()
        {

        }

        public RGB(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public RGB Clamp()
        {
            RGB rgb = new RGB(R, G, B);

            if (rgb.R < 0)
                rgb.R = 0;
            if (rgb.R > 1)
                rgb.R = 1;

            if (rgb.G < 0)
                rgb.G = 0;
            if (rgb.G > 1)
                rgb.G = 1;

            if (rgb.B < 0)
                rgb.B = 0;
            if (rgb.B > 1)
                rgb.B = 1;

            return rgb;
        }
    }
}
