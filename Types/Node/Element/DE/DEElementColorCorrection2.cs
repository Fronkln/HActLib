using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
   // [ElementID(Game.LJ, 0x1C9)]
  //  [ElementID(Game.LAD7Gaiden, 0x1C9)]
  //  [ElementID(Game.LADIW, 0x1C9)]
    public class DEElementColorCorrection2 : NodeElement
    {
        public uint Size;
        public float[] Animation = new float[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            long pos = reader.Stream.Position;

            Size = reader.ReadUInt32();
            Animation = new float[32];

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Size);

            foreach (float f in Animation)
                writer.Write(f);
        }
    }
}
