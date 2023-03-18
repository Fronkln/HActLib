using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x126)]
    [ElementID(Game.YK2, 0x126)]
    [ElementID(Game.JE, 0x126)]
    [ElementID(Game.YLAD, 0x120)]
    [ElementID(Game.LJ, 0x120)]
    [ElementID(Game.Gaiden, 0x120)]
    [ElementID(Game.Y8, 0x120)]
    public class DEElementAnalogNoise : NodeElement
    {
        public float Speed = 0.1f;
        public float Scale = 0.3f;
        public float Threshold = 0;
        public float Intensity = 500;
        public float Power = 1;
        public uint Fix = 0;
        public float FixPos = 0;
        public uint Seed = 1481;
        public float Persistence = 0.36f;

        public uint UnkVal;

        public byte[] Animation = new byte[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Speed = reader.ReadSingle();
            Scale = reader.ReadSingle();
            Threshold = reader.ReadSingle();
            Intensity = reader.ReadSingle();
            Power = reader.ReadSingle();
            Fix = reader.ReadUInt32();
            FixPos = reader.ReadSingle();
            Seed = reader.ReadUInt32();
            Persistence = reader.ReadSingle();

            UnkVal = reader.ReadUInt32();
            reader.ReadBytes(8);
            
            Animation = reader.ReadBytes(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Speed);
            writer.Write(Scale);
            writer.Write(Threshold);
            writer.Write(Intensity);
            writer.Write(Power);
            writer.Write(Fix);
            writer.Write(FixPos);
            writer.Write(Seed);
            writer.Write(Persistence);

            writer.Write(UnkVal);
            writer.WriteTimes(0, 8);

            writer.Write(Animation);
        }
    }
}
