using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.YLAD, 0x30)]
    [ElementID(Game.LJ, 0x30)]
    [ElementID(Game.LAD7Gaiden, 0x30)]
    [ElementID(Game.LADIW, 0x30)]
    [ElementID(Game.LADPYIH, 0x30)]
    public class DEElementEyesHighlight : NodeElement
    {
        public Vector2 Pos1;
        public float Radius1;
        public float Strength1;

        public Vector2 Pos2;
        public float Radius2;
        public float Strength2;

        public Vector2 Pos3;
        public float Radius3;
        public float Strength3;
        public float Length3;
        public float Rotate3;
        public int HighlightType3;

        public float ForrowRatio;

        public RGB32 Color1;
        public int Kelvin1;
        public float Luminance1;

        public RGB32 Color2;
        public int Kelvin2;
        public float Luminance2;

        public RGB32 Color3;
        public int Kelvin3;
        public float Luminance3;

        public float Reflectance;
        public float AlbedoScale;

        public int ReflectionControl;

        public float Unknown1 = 1f;
        public int Unknown2 = 2;
        public float Unknown3 = 0;
        public float Unknown4 = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Pos1 = reader.ReadVector2();
            Radius1 = reader.ReadSingle();
            Strength1 = reader.ReadSingle();

            Pos2 = reader.ReadVector2();
            Radius2 = reader.ReadSingle();
            Strength2 = reader.ReadSingle();

            Pos3 = reader.ReadVector2();
            Radius3 = reader.ReadSingle();
            Strength3 = reader.ReadSingle();
            Length3 = reader.ReadSingle();
            Rotate3 = reader.ReadSingle();
            HighlightType3 = reader.ReadInt32();

            ForrowRatio = reader.ReadSingle();

            Color1 = reader.ReadRGB32();
            Kelvin1 = reader.ReadInt32();
            Luminance1 = reader.ReadSingle();

            Color2 = reader.ReadRGB32();
            Kelvin2 = reader.ReadInt32();
            Luminance2 = reader.ReadSingle();

            Color3 = reader.ReadRGB32();
            Kelvin3 = reader.ReadInt32();
            Luminance3 = reader.ReadSingle();

            Reflectance = reader.ReadSingle();
            AlbedoScale = reader.ReadSingle();

            ReflectionControl = reader.ReadInt32();

            if(CMN.LastHActDEGame >= Game.LJ)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadInt32();
                Unknown3 = reader.ReadSingle();
                Unknown4 = reader.ReadSingle();
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Pos1);
            writer.Write(Radius1);
            writer.Write(Strength1);

            writer.Write(Pos2);
            writer.Write(Radius2);
            writer.Write(Strength2);

            writer.Write(Pos3);
            writer.Write(Radius3);
            writer.Write(Strength3);
            writer.Write(Length3);
            writer.Write(Rotate3);
            writer.Write(HighlightType3);

            writer.Write(ForrowRatio);

            writer.Write(Color1);
            writer.Write(Kelvin1);
            writer.Write(Luminance1);

            writer.Write(Color2);
            writer.Write(Kelvin2);
            writer.Write(Luminance2);

            writer.Write(Color3);
            writer.Write(Kelvin3);
            writer.Write(Luminance3);

            writer.Write(Reflectance);
            writer.Write(AlbedoScale);

            writer.Write(ReflectionControl);

            if(CMN.LastHActDEGame >= Game.LJ)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }
    }
}
