using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
 //   [ElementID(Game.Y6, 0x17)]
  //  [ElementID(Game.YK2, 0x17)]
  //  [ElementID(Game.JE, 0x17)]
  //  [ElementID(Game.YLAD, 0x16)]
  //  [ElementID(Game.LJ, 0x16)]
  //  [ElementID(Game.Y8, 0x16)]
    public class DEElementColorCorrection : NodeElement
    {
        public float Interpolation;
        public float Contrast;
        public float Saturation;

        public RGB Skin;
        public float SkinSaturation;
        public RGB Chara;
        public float CharaSaturation;
        public float[] AnimationData = new float[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            // base.ReadElementData(reader, inf, version);

            long colorOffset = reader.ReadInt64();
            uint texSize = reader.ReadUInt32();
            
            Interpolation = reader.ReadSingle();
            Contrast = reader.ReadSingle();
            Saturation = reader.ReadSingle();

            Skin = reader.Read<RGB>();
            SkinSaturation = reader.ReadSingle();
           
            Chara = reader.Read<RGB>();    
            CharaSaturation = reader.ReadSingle();

            //reader.ReadBytes(8);

            for (int i = 0; i < AnimationData.Length; i++)
                AnimationData[i] = reader.ReadSingle();
        }
    }
}
