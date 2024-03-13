using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.JE, 0x172)]
    [ElementID(Game.YLAD, 0x170)]
    [ElementID(Game.LJ, 0x170)]
    [ElementID(Game.LAD7Gaiden, 0x170)]
    [ElementID(Game.LADIW, 0x170)]
    public class DEElementDOF2 : NodeElement
    {
        public bool DisableDof;

        public float FocusDistBefore;
        public float FocusDistAfter;

        public bool UseIntr;

        public float FarCocRate;
        public float NearCocRate;

        public float FocusFarD;
        public float FarMaxCocDist;
        public float FocusNearD;
        public float NearMaxCocDist;
        public float MaxCocRadius;

        public float SpecularBrightness;
        public float SpecularThresold;

        public float Alpha;
        public float ColorScale;

        public int Shape;

        public int DiaphragmBladesNum;
        public int ShapeRotAngle;
        public float ApertureCircularity;
        public float BokehAttenBeginRate;
        public float HighLumiEmphasisThresold;
        public float HighLumiEmphasisColor;
        public float HighLumiEmphasisScale;

        public bool FullResoBokeh;
        public int NearFocusDistance;
        public float FNumber;
        public bool UseFNumber;
        public bool EnableFocusAdjust;
        public float FocusNearDFNumber;
        public float FocusFarDFNumber;
        public bool CalcBeforeTAA;
        public float MaxCocRadiusF;

        public int CinematicDofQualityLevel;
        public bool EnableMirror;
        public bool EnableMirrorOnly;
        public int ScatterMode;
        public int RingCount;
        public int TAAQuality;
        public int RecombineQuality;
        public int DOFFlag;

        public float[] Animation = new float[32];

        public int Unknown1;
        public int Unknown2 = 2;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            DisableDof = reader.ReadInt32() > 0;

            FocusDistBefore = reader.ReadSingle();
            FocusDistAfter = reader.ReadSingle();

            UseIntr = reader.ReadInt32() > 0;

            FarCocRate = reader.ReadSingle();
            NearCocRate = reader.ReadSingle();

            FocusFarD = reader.ReadSingle();
            FarMaxCocDist = reader.ReadSingle();
            FocusNearD = reader.ReadSingle();
            NearMaxCocDist = reader.ReadSingle();
            MaxCocRadius = reader.ReadSingle();

            SpecularBrightness = reader.ReadSingle();
            SpecularThresold = reader.ReadSingle();

            Alpha = reader.ReadSingle();
            ColorScale = reader.ReadSingle();

            Shape = reader.ReadInt32();

            DiaphragmBladesNum = reader.ReadInt32();
            ShapeRotAngle = reader.ReadInt32();
            ApertureCircularity = reader.ReadSingle();
            BokehAttenBeginRate = reader.ReadSingle();
            HighLumiEmphasisThresold = reader.ReadSingle();
            HighLumiEmphasisColor = reader.ReadSingle();
            HighLumiEmphasisScale = reader.ReadSingle();

            FullResoBokeh = reader.ReadInt32() > 0;
            NearFocusDistance = reader.ReadInt32();
            FNumber = reader.ReadSingle();
            UseFNumber = reader.ReadInt32() > 0;
            EnableFocusAdjust = reader.ReadInt32() > 0;
            FocusNearDFNumber = reader.ReadSingle();
            FocusFarDFNumber = reader.ReadSingle();
            CalcBeforeTAA = reader.ReadInt32() > 0;
            MaxCocRadiusF = reader.ReadSingle();

            CinematicDofQualityLevel = reader.ReadInt32();
            EnableMirror = reader.ReadInt32() > 0;
            EnableMirrorOnly = reader.ReadInt32() > 0;
            ScatterMode = reader.ReadInt32();
            RingCount = reader.ReadInt32();
            TAAQuality = reader.ReadInt32();
            RecombineQuality = reader.ReadInt32();

            if (CMN.LastHActDEGame > Game.LJ)
            {
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
            }

            DOFFlag = reader.ReadInt32();

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Convert.ToInt32(DisableDof));

            writer.Write(FocusDistBefore);
            writer.Write(FocusDistAfter);

            writer.Write(Convert.ToInt32(UseIntr));

            writer.Write(FarCocRate);
            writer.Write(NearCocRate);

            writer.Write(FocusFarD);
            writer.Write(FarMaxCocDist);
            writer.Write(FocusNearD);
            writer.Write(NearMaxCocDist);
            writer.Write(MaxCocRadius);

            writer.Write(SpecularBrightness);
            writer.Write(SpecularThresold);

            writer.Write(Alpha);
            writer.Write(ColorScale);

            writer.Write(Shape);

            writer.Write(DiaphragmBladesNum);
            writer.Write(ShapeRotAngle);
            writer.Write(ApertureCircularity);
            writer.Write(BokehAttenBeginRate);
            writer.Write(HighLumiEmphasisThresold);
            writer.Write(HighLumiEmphasisColor);
            writer.Write(HighLumiEmphasisScale);

            writer.Write(Convert.ToInt32(FullResoBokeh));
            writer.Write(NearFocusDistance);
            writer.Write(FNumber);
            writer.Write(Convert.ToInt32(UseFNumber));
            writer.Write(Convert.ToInt32(EnableFocusAdjust));
            writer.Write(FocusNearDFNumber);
            writer.Write(FocusFarDFNumber);
            writer.Write(Convert.ToInt32(CalcBeforeTAA));
            writer.Write(MaxCocRadiusF);

            writer.Write(CinematicDofQualityLevel);
            writer.Write(Convert.ToInt32(EnableMirror));
            writer.Write(Convert.ToInt32(EnableMirrorOnly));
            writer.Write(ScatterMode);
            writer.Write(RingCount);
            writer.Write(TAAQuality);
            writer.Write(RecombineQuality);

            if (CMN.LastHActDEGame > Game.LJ)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
            }

            writer.Write(DOFFlag);

            for (int i = 0; i < Animation.Length; i++)
                writer.Write(Animation[i]);
        }
    }
}
