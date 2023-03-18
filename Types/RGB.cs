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
    }
}
