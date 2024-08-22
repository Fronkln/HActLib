using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x20)]
    [ElementID(Game.Ishin, 0x20)]
    [ElementID(Game.Y0, 0x20)]
    [ElementID(Game.YK1, 0x20)]
    public class OEDamage : NodeElement
    {
        public ushort Unk1 = 1;
        public ushort Unk2 = 3;

        public int Damage = 200;
        public bool NoDead = false;

        private byte[] unkDat1 = new byte[8];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadUInt16();
            Unk2 = reader.ReadUInt16();
            Damage = reader.ReadInt32();
            unkDat1 = reader.ReadBytes(8);
            NoDead = reader.ReadUInt32() > 0;
            reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Damage);
            writer.Write(unkDat1);
            writer.Write(Convert.ToInt32(NoDead));
            writer.WriteTimes(0, 12);
        }
    }
}
