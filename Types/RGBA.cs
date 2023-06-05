using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public struct RGBA
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public RGBA(byte r, byte g, byte b , byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator System.Drawing.Color(RGBA col)
        {
            return System.Drawing.Color.FromArgb(col.a, col.r,col.g,col.b);
        }

        public static implicit operator RGBA(System.Drawing.Color col)
        {
            return new RGBA() { r = col.R, g = col.G, b = col.B, a = col.A };
        }

        public static RGBA Read(DataReader reader)
        {
            return new RGBA(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public void Write(DataWriter writer)
        {
            writer.Write(r);
            writer.Write(g);
            writer.Write(b);
            writer.Write(a);
        }
    }
}
