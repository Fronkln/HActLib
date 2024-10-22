using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x58)]
    [ElementID(Game.YK2, 0x58)]
    [ElementID(Game.JE, 0x58)]
    [ElementID(Game.YLAD, 0x55)]
    [ElementID(Game.LJ, 0x55)]
    [ElementID(Game.LAD7Gaiden, 0x55)]
    [ElementID(Game.LADIW, 0x55)]
    [ElementID(Game.LADPYIH, 0x55)]
    public class DEElementCameraShake : NodeElement
    {
        public CameraShakeType Type;
        public float Scale;
        public float Speed;
        public int PriorityID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Type = (CameraShakeType)reader.ReadUInt32();
            Scale = reader.ReadSingle();
            Speed = reader.ReadSingle();
            PriorityID = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write((uint)Type);
            writer.Write(Scale);
            writer.Write(Speed);
            writer.Write(PriorityID);
        }
    }
}
