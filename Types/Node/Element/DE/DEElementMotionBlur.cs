using HActLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x1E)]
    [ElementID(Game.YK2, 0x1E)]
    [ElementID(Game.JE, 0x1E)]
    public class DEElementMotionBlur : NodeElement
    {
        public float ShutterSpeed = 30;
        public float BlurLength = 0.1f;
        public float SamplingInterleave = 1f;
        public float FalloffStartDist = 0f;
        public float FalloffEndDist = 0f;
        public float RotationPerspectiveThreshold = 0.02f;
        public float TransThreshold = 0f;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ShutterSpeed = reader.ReadSingle();
            BlurLength = reader.ReadSingle();
            SamplingInterleave = reader.ReadSingle();
            FalloffStartDist = reader.ReadSingle();
            FalloffEndDist = reader.ReadSingle();
            RotationPerspectiveThreshold = reader.ReadSingle();
            TransThreshold = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(ShutterSpeed);
            writer.Write(BlurLength);
            writer.Write(SamplingInterleave);
            writer.Write(FalloffStartDist);
            writer.Write(FalloffEndDist);
            writer.Write(RotationPerspectiveThreshold);
            writer.Write(TransThreshold);
        }


        public override Node TryConvert(Game input, Game output)
        {
            GameVersion outputVer = CMN.GetVersionForGame(output);

            if(outputVer >= GameVersion.DE2)
            {
                DEElementMotionBlur2 blur2 = new DEElementMotionBlur2();
                blur2.ShutterSpeed = ShutterSpeed;
                blur2.BlurLength = BlurLength;

                blur2.ElementKind = Reflection.GetElementIDByName("e_auth_element_post_effect_motion_blur2", output);
                blur2.BEPDat.PropertyType = (ushort)blur2.ElementKind;

                return blur2;
            }

            return this;
        }
    }
}
