using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x17)]
    [ElementID(Game.YK2, 0x17)]
    [ElementID(Game.JE, 0x17)]
    [ElementID(Game.YLAD, 0x16)]
    [ElementID(Game.LJ, 0x16)]
    [ElementID(Game.LADIW, 0x16)]
    public class DEElementColorCorrection : NodeElement
    {
        public uint Size;
        public float InterpolateSec;

        public bool UseCurve;
        public bool UseSaturation;

        public float Contrast;
        public float SaturationControl;

        public RGB Skin = new RGB(1, 1, 1);
        public float SkinSaturation;

        public RGB Chara = new RGB(1, 1, 1);
        public float CharaSaturation;

        public bool IsOverwrite;

        public float[] Animation = new float[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Size = reader.ReadUInt32();
            InterpolateSec = reader.ReadSingle();

            UseCurve = reader.ReadInt32() > 0;

            if (version > GameVersion.DE1)
            {

                UseSaturation = reader.ReadInt32() > 0;

                Contrast = reader.ReadSingle();
                SaturationControl = reader.ReadSingle();

                Skin = reader.ReadRGB();
                SkinSaturation = reader.ReadSingle();

                Chara = reader.ReadRGB();
                CharaSaturation = reader.ReadSingle();

                IsOverwrite = reader.ReadInt32() > 0;
            }

            Animation = new float[32];

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Size);
            writer.Write(InterpolateSec);

            writer.Write(Convert.ToInt32(UseCurve));

            if (version > GameVersion.DE1)
            {
                writer.Write(Convert.ToInt32(UseSaturation));

                writer.Write(Contrast);
                writer.Write(SaturationControl);

                writer.Write(Skin);
                writer.Write(SkinSaturation);

                writer.Write(Chara);
                writer.Write(CharaSaturation);

                writer.Write(Convert.ToInt32(IsOverwrite));
            }

            foreach (float f in Animation)
                writer.Write(f);
        }

        public byte[] ToBitmapBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    byte[] bmpHeader2 = null;


                    switch (Size)
                    {
                        default:
                            bmpHeader2 = new byte[] { 0x42, 0x4D, 0x36, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x36, 0x0, 0x0, 0x0, 0x28, 0x0, 0x0, 0x0, 0x0, 0x4, 0x0, 0x0, (byte)Size, 0x0, 0x0, 0x0, 0x1, 0x0, 0x18, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                            break;
                        case 32:
                            bmpHeader2 = new byte[] { 0x42, 0x4D, 0x36, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x36, 0x0, 0x0, 0x0, 0x28, 0x0, 0x0, 0x0, 0x0, 0x4, 0x0, 0x0, (byte)Size, 0x0, 0x0, 0x0, 0x1, 0x0, 0x18, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                            break;
                        case 16:
                            bmpHeader2 = new byte[] { 0x42, 0x4D, 0x36, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x36, 0x0, 0x0, 0x0, 0x28, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, (byte)Size, 0x0, 0x0, 0x0, 0x1, 0x0, 0x18, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x30, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                            break;
                    }

                    bw.Write(bmpHeader2);
                    bw.Write(unkBytes);
                }

                return ms.GetBuffer();
            }
        }

        public void ExportToBMP(string path)
        {
            File.WriteAllBytes(path, ToBitmapBytes());
        }
    }
}
