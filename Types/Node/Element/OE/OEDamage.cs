using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x20)]
    [ElementID(Game.Ishin, 0x20)]
    [ElementID(Game.Y0, 0x20)]
    [ElementID(Game.YK1, 0x20)]
    [ElementID(Game.FOTNS, 0x21)]
    public class OEDamage : NodeElement
    {
        public ushort Unk1 = 1;
        public ushort Unk2 = 3;
        public int ForceDead = 0;
        public int Vanish = 0;

        public int Damage = 200;
        public bool NoDead = false;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadUInt16();
            Unk2 = reader.ReadUInt16();
            Damage = reader.ReadInt32();
            ForceDead = reader.ReadInt32();
            Vanish = reader.ReadInt32();
            NoDead = reader.ReadUInt32() > 0;
            //reader.ReadBytes(12);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Damage);
            writer.Write(ForceDead);
            writer.Write(Vanish);
            writer.Write(Convert.ToInt32(NoDead));
           // writer.WriteTimes(0, 12);
        }
    }
}
