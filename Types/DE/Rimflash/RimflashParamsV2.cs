using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class RimflashParamsV2 : RimflashParams
    {
        public int Pattern { get; set; } = 0;
        public float AdjustDecompressLuminance { get; set; } = 0;
        public float AdjustDecompressLuminanceOpaque { get; set; } = 0.3f;

        internal override void Read(DataReader reader)
        {
            base.Read(reader);

            Pattern = reader.ReadInt32();
            AdjustDecompressLuminance = reader.ReadSingle();
            AdjustDecompressLuminanceOpaque = reader.ReadSingle();
        }

        internal override void Write(DataWriter writer)
        {
            base.Write(writer);

            writer.Write(Pattern);
            writer.Write(AdjustDecompressLuminance);
            writer.Write(AdjustDecompressLuminanceOpaque);
        }
    }
}
