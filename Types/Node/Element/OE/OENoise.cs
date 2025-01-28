using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 12)]
    [ElementID(Game.Ishin, 12)]
    [ElementID(Game.Y0, 12)]
    [ElementID(Game.FOTNS, 13)]
    public class OENoise : OEBaseEffect
    {
        public float Power = 1;
        public float Size = 1;
        public float NoiseUnk3 = 1;
        public uint Intensity = 1;

        private byte[] unkdat1 = new byte[16];

        public byte[] Animation = new byte[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            Power = reader.ReadSingle();
            Size = reader.ReadSingle();
            NoiseUnk3 = reader.ReadSingle();
            Intensity = reader.ReadUInt32();
            unkdat1 = reader.ReadBytes(16);
            Animation = reader.ReadBytes(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);

            writer.Write(Power);
            writer.Write(Size);
            writer.Write(NoiseUnk3);
            writer.Write(Intensity);
            writer.Write(unkdat1);
            writer.Write(Animation);
        }
    }
}
