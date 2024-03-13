using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.YLAD, 0x172)]
    [ElementID(Game.LJ, 0x172)]
    [ElementID(Game.LAD7Gaiden, 0x172)]
    [ElementID(Game.LADIW, 0x172)]
    public class DEElementMotionBlur2 : NodeElement
    {
        public float ShutterSpeed = 30;
        public float BlurLength = 0.2f;
        public int SampleCount = 8;
        public float Unknown = 0.4f;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ShutterSpeed = reader.ReadSingle();
            BlurLength = reader.ReadSingle();
            SampleCount = reader.ReadInt32();

            if (inf.expectedSize > 12)
                Unknown = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(ShutterSpeed);
            writer.Write(BlurLength);
            writer.Write(SampleCount);

            if (CMN.LastHActDEGame >= Game.LADIW)
                writer.Write(Unknown);
        }
    }
}
