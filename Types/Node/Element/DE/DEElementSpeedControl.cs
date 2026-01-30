using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public enum SpeedType
    {
        General,
        Player,
        Character,
        Camera,
        UI,
        Unprocessed
    }

    [ElementID(Game.Y6, 0x5F)]
    [ElementID(Game.YK2, 0x5F)]
    [ElementID(Game.JE, 0x5F)]
    [ElementID(Game.YLAD, 0x5C)]
    [ElementID(Game.LJ, 0x5C)]
    [ElementID(Game.LAD7Gaiden, 0x5C)]
    [ElementID(Game.LADIW, 0x5C)]
    [ElementID(Game.LADPYIH, 0x5C)]
    [ElementID(Game.YK3, 0x5C)]
    public class DEElementSpeedControl : NodeElement
    {
        public SpeedType Type;
        public float MinSpeed;
        public float MaxSpeed;
        public bool IsUseUnprocess;
        public uint FrameNum;
        public float[] AnimData = new float[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Type = (SpeedType)reader.ReadUInt32();
            MaxSpeed = reader.ReadSingle();
            MinSpeed = reader.ReadSingle();
            FrameNum = reader.ReadUInt32();
            IsUseUnprocess = reader.ReadInt32() == 1;
            reader.ReadBytes(12);

            for (int i = 0; i < AnimData.Length; i++)
                AnimData[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write((uint)Type);
            writer.Write(MaxSpeed);
            writer.Write(MinSpeed);
            writer.Write(FrameNum);
   
            writer.Write(Convert.ToInt32(IsUseUnprocess));
            writer.WriteTimes(0, 12);

            for (int i = 0; i < AnimData.Length; i++)
                writer.Write(AnimData[i]);
        }
    }
}
