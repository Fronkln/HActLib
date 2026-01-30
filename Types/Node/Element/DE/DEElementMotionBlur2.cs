using HActLib.Internal;
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
    [ElementID(Game.LADPYIH, 0x172)]
    [ElementID(Game.YK3, 0x172)]
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

            //Spotted: LADIW
            if (inf.expectedSize > 44)
                Unknown = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(ShutterSpeed);
            writer.Write(BlurLength);
            writer.Write(SampleCount);

            if (CMN.LastHActDEGame >= Game.LADIW)
                writer.Write(Unknown);
        }

        public override Node TryConvert(Game input, Game output)
        {
            GameVersion outputVer = CMN.GetVersionForGame(output);

            if (outputVer <= GameVersion.DE1)
            {
                DEElementMotionBlur blur = new DEElementMotionBlur();
                blur.ShutterSpeed = ShutterSpeed;
                blur.BlurLength = BlurLength;

                blur.ElementKind = Reflection.GetElementIDByName("e_auth_element_post_effect_motion_blur", output);
                blur.BEPDat.PropertyType = (ushort)blur.ElementKind;

                return blur;
            }

            return this;
        }
    }
}
