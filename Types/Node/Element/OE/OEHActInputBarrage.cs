using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x2C)]
    [ElementID(Game.Ishin, 0x2C)]
    [ElementID(Game.Y0, 0x2C)]
    [ElementID(Game.YK1, 0x2C)]
    public class OEHActInputBarrage : NodeElement
    {
        public uint Presses;
        public uint Unk1;
        public uint Unk2;
        public uint Unk3;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            reader.ReadBytes(8);

            Unk1 = reader.ReadUInt32();
            Presses = reader.ReadUInt32();
            Unk2= reader.ReadUInt32();
            Unk3 = reader.ReadUInt32();
          
            reader.ReadBytes(4);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.WriteTimes(0, 8);

            writer.Write(Unk1);
            writer.Write(Presses);
            writer.Write(Unk2);
            writer.Write(Unk3);

            writer.WriteTimes(0, 4);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            if (hactVer <= 10)
                return 0xB;
            else
                new Exception("Implement this Jhrino");

            return base.GetSize(version, hactVer);
        }
    }
}
