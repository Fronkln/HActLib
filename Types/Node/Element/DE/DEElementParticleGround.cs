using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x16)]
    [ElementID(Game.YK2, 0x16)]
    [ElementID(Game.JE, 0x16)]
    [ElementID(Game.YLAD, 0x15)]
    [ElementID(Game.LJ, 0x15)]
    [ElementID(Game.LAD7Gaiden, 0x15)]
    [ElementID(Game.LADIW, 0x15)]
    [ElementID(Game.LADPYIH, 0x15)]
    [ElementID(Game.YK3, 0x15)]
    public class DEElementParticleGround : NodeElement
    {
        public Vector3 Scale = new Vector3(1, 1, 1);
        public RGBA32 Color = new RGBA32(255, 255, 255, 255);

        public float AlphaScale = 1;
        public float TimeScale = 1;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Scale = reader.ReadVector3();
            Color = reader.ReadRGBA32();

            AlphaScale = reader.ReadSingle();
            TimeScale = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Scale);
            writer.Write(Color);

            writer.Write(AlphaScale);
            writer.Write(TimeScale);
        }
    }
}
