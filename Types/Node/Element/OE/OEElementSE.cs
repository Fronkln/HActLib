using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x21)]
    [ElementID(Game.Ishin, 0x21)]
    [ElementID(Game.Y0, 0x21)]
    [ElementID(Game.YK1, 0x21)]
    [ElementID(Game.FOTNS, 0x22)]
    public class OEElementSE : NodeElement
    {
        public ushort Cuesheet;
        public ushort Sound;

        public uint Flags = 0;
        public float CustomDecayNearDist = 4;
        public float CustomDecayNearVol = 1;
        public float CustomDecayFarDist = 40;
        public float CustomDecayFarVol = 0.01f;
        public int Volume = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Cuesheet = reader.ReadUInt16();
            Sound = reader.ReadUInt16();

            Flags = reader.ReadUInt32();
            CustomDecayNearDist = reader.ReadSingle();
            CustomDecayNearVol = reader.ReadSingle();
            CustomDecayFarDist = reader.ReadSingle();
            CustomDecayFarVol = reader.ReadSingle();
            Volume = reader.ReadInt32();

            reader.ReadBytes(4);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Cuesheet);
            writer.Write(Sound);

            writer.Write(Flags);
            writer.Write(CustomDecayNearDist);
            writer.Write(CustomDecayNearVol);
            writer.Write(CustomDecayFarDist);
            writer.Write(CustomDecayFarVol);
            writer.Write(Volume);

            writer.WriteTimes(0, 4);
        }


        public bool IsGVSound()
        {
            //Between 0x8000 and 0x8999
            return Cuesheet >= 32768 && Cuesheet < 36864;
        }

        public int GetGVCategory()
        {

            if (!IsGVSound())
                return 0;
            return Cuesheet - 32768;
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            if (hactVer <= 10)
                return 0xC;
            else
                return 0x10;
        }
    }
}
