using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x24)]
    [ElementID(Game.YK2, 0x24)]
    [ElementID(Game.JE, 0x24)]
    [ElementID(Game.YLAD, 0x22)]
    [ElementID(Game.LJ, 0x22)]
    [ElementID(Game.LAD7Gaiden, 0x22)]
    [ElementID(Game.LADIW, 0x22)]
    [ElementID(Game.LADPYIH, 0x22)]
    public class DEElementDirectionalLight : DEElementBaseLight
    {
        public float Luminance;
        public int LightFlags;
        public float RotateY;
        public float[] Animation; //DE2 only

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);
            Luminance = reader.ReadSingle();
            LightFlags = reader.ReadInt32();
            RotateY = reader.ReadSingle();

            if(version >= GameVersion.DE2)
            {
                Animation = new float[256];

                for(int i = 0; i <  Animation.Length; i++)
                {
                    Animation[i] = reader.ReadSingle();
                }
            }    
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);
            writer.Write(Luminance);
            writer.Write(LightFlags);
            writer.Write(RotateY);

            if(version >= GameVersion.DE2)
                foreach(float f in  Animation)
                    writer.Write(f);
        }
    }
}
