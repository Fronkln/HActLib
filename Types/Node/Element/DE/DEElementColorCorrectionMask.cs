using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x106)]
    [ElementID(Game.YK2, 0x106)]
    [ElementID(Game.JE, 0x106)]
    [ElementID(Game.YLAD, 0x101)]
    [ElementID(Game.LJ, 0x101)]
    [ElementID(Game.Gaiden, 0x101)]
    [ElementID(Game.Y8, 0x101)]
    public class DEElementColorCorrectionMask : NodeElement
    {
        public float Hue;
        public float Saturation;
        public float Contrast;
        public float Luminance;
        public bool DisableCharacter;
        public byte[] Animation = new byte[32];
        public uint MaskPriority;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Hue = reader.ReadSingle();
            Saturation = reader.ReadSingle();
            Contrast = reader.ReadSingle();
            Luminance = reader.ReadSingle();
            DisableCharacter = reader.ReadUInt32() == 1;
            Animation = reader.ReadBytes(32);
            MaskPriority = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Hue);
            writer.Write(Saturation);
            writer.Write(Contrast);
            writer.Write(Luminance);
            writer.Write(Convert.ToUInt32(DisableCharacter));
            writer.Write(Animation);
            writer.Write(MaskPriority);
        }
    }
}
