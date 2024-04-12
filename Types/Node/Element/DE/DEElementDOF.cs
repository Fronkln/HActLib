using HActLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x8)]
    [ElementID(Game.YK2, 0x8)]
    [ElementID(Game.JE, 0x8)]
    public class DEElementDOF : NodeElement
    {
        public bool DisableDof = false;
        public bool UseIntr = false;
        public int Shape = 7;
        public int DiaphragmBladesNum = 6;
        public float ApertureCircularity = 0;

        public float FocusDistBefore = 1;
        public float FocusDistAfter = 1;

        public int EdgeType = -1;
        public float EdgeThreshold = 0.1f;

        public int Quality = 5;
        public int FocusLocalPos = 0;

        public float AlphaToCoverageDepthThreshold;

        public float[] Animation = new float[32];

        public int LensType;

        public float Aberration;
        public float AberrationFOV;

        public float GradientThreshold;
        public float GradientMinThreshold;
        public float GradientMaxThreshold;

        public int NearFocusDistance;

        public float DOFAfterDisableDist;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            DisableDof = reader.ReadInt32() > 0;
            UseIntr = reader.ReadInt32() > 0;
            
            Shape = reader.ReadInt32();
            DiaphragmBladesNum = reader.ReadInt32();
            ApertureCircularity = reader.ReadSingle();

            FocusDistBefore = reader.ReadSingle();
            FocusDistAfter = reader.ReadSingle();

            EdgeType = reader.ReadInt32();
            EdgeThreshold = reader.ReadSingle();

            Quality = reader.ReadInt32();
            FocusLocalPos = reader.ReadInt32();
            AlphaToCoverageDepthThreshold = reader.ReadSingle();

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();

            LensType = reader.ReadInt32();
            Aberration = reader.ReadSingle();
            AberrationFOV = reader.ReadSingle();
            GradientThreshold = reader.ReadSingle();
            GradientMinThreshold = reader.ReadSingle();
            GradientMaxThreshold = reader.ReadSingle();

            NearFocusDistance = reader.ReadInt32();
            DOFAfterDisableDist = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Convert.ToInt32(DisableDof));
            writer.Write(Convert.ToInt32(UseIntr));

            writer.Write(Shape);
            writer.Write(DiaphragmBladesNum);
            writer.Write(ApertureCircularity);

            writer.Write(FocusDistBefore);
            writer.Write(FocusDistAfter);

            writer.Write(EdgeType);
            writer.Write(EdgeThreshold);

            writer.Write(Quality);
            writer.Write(FocusLocalPos);
            writer.Write(AlphaToCoverageDepthThreshold);

            for (int i = 0; i < 32; i++)
                writer.Write(Animation[i]);

            writer.Write(LensType);
            writer.Write(Aberration);
            writer.Write(AberrationFOV);
            writer.Write(GradientThreshold);
            writer.Write(GradientMinThreshold);
            writer.Write(GradientMaxThreshold);

            writer.Write(NearFocusDistance);
            writer.Write(DOFAfterDisableDist);
        }

        public override Node TryConvert(Game input, Game output)
        {
            GameVersion outputVer = CMN.GetVersionForGame(output);

            if (outputVer >= GameVersion.DE2)
            {
                DEElementDOF2 blur2 = new DEElementDOF2();

                blur2.DisableDof = DisableDof;
                blur2.UseIntr = UseIntr;

                blur2.Shape = Shape;
                blur2.DiaphragmBladesNum = DiaphragmBladesNum;
                blur2.ApertureCircularity = ApertureCircularity;

                if(FocusDistBefore == 1 && FocusDistAfter == 1)
                {
                    blur2.FocusDistBefore = 0;
                    blur2.FocusDistAfter = 0;
                }
                else
                {
                    blur2.FocusDistBefore = FocusDistBefore;
                    blur2.FocusDistAfter = FocusDistAfter;
                }

                blur2.NearFocusDistance = NearFocusDistance;
                blur2.Animation = Animation;

                blur2.ElementKind = Reflection.GetElementIDByName("e_auth_element_post_effect_dof2", output);
                blur2.BEPDat.PropertyType = (ushort)blur2.ElementKind;

                return blur2;
            }

            return this;
        }
    }
}
