using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 2)]
    [ElementID(Game.Ishin, 2)]
    [ElementID(Game.Y0, 2)]
    public class OEParticle : NodeElement
    {
        public uint ParticleID = 0;
        public uint Unknown; //always 15, between y5 and y0

        private byte[] unkDat1;

        public float Unknown1;
        public int Unknown2;
        public float Unknown3;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ParticleID = reader.ReadUInt32();
            Unknown = reader.ReadUInt32();
            unkDat1 = reader.ReadBytes(8);
            Unknown1 = reader.ReadSingle();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(ParticleID);
            writer.Write(Unknown);
            writer.Write(unkDat1);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
        }
    }
}
