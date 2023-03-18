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
    [ElementID(Game.YLAD, 0x121)]
    [ElementID(Game.LJ, 0x121)]
    [ElementID(Game.Gaiden, 0x121)]
    [ElementID(Game.Y8, 0x121)]
    public class DEElementGrainNoise : NodeElement
    {
        public float Power = 1;
        public float Size = 1;
        public uint Intensity = 1;

        public byte[] Animation = new byte[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Power = reader.ReadSingle();
            Size = reader.ReadSingle();
            Intensity = reader.ReadUInt32();

            reader.ReadInt32();

            Animation = reader.ReadBytes(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Power);
            writer.Write(Size);
            writer.Write(Intensity);

            writer.WriteTimes(0, 4);

            writer.Write(Animation);
        }
    }
}
