using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0xAF)]
    [ElementID(Game.YK2, 0xAF)]
    [ElementID(Game.JE, 0xAF)]
    [ElementID(Game.YLAD, 0xAB)]
    [ElementID(Game.LJ, 0xAB)]
    [ElementID(Game.LAD7Gaiden, 0xAB)]
    [ElementID(Game.LADIW, 0xAB)]
    [ElementID(Game.LADPYIH, 0xAB)]
    [ElementID(Game.YK3, 0xAB)]
    public class DEElementChromaticAberration : NodeElement
    {
        public int Samples;
        public float MagnificationDispersion;
        public float UniformDispersionX;
        public float UniformDispersionY;
        public float MagnificationDispersionAfter;
        public float UniformDispersionXAfter;
        public float UniformDispersionYAfter;

        public float[] Animation;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Samples = reader.ReadInt32();
            
            MagnificationDispersion = reader.ReadSingle();
            UniformDispersionX = reader.ReadSingle();
            UniformDispersionY = reader.ReadSingle();
            
            MagnificationDispersionAfter = reader.ReadSingle();
            UniformDispersionXAfter = reader.ReadSingle();
            UniformDispersionYAfter = reader.ReadSingle();

            Animation = new float[32];

            for(int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Samples);

            writer.Write(MagnificationDispersion);
            writer.Write(UniformDispersionX);
            writer.Write(UniformDispersionY);

            writer.Write(MagnificationDispersionAfter);
            writer.Write(UniformDispersionXAfter);
            writer.Write(UniformDispersionYAfter);

            for (int i = 0; i < 32; i++)
                writer.Write(Animation[i]);
        }
    }
}
