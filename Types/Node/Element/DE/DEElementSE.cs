using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{

    public enum Versions
    {
        Unknown,
        Y7AndBelow,
        LJAndAbove,
    }

    [ElementID(Game.Y6, 0x1A)]
    [ElementID(Game.YK2, 0x1A)]
    [ElementID(Game.JE, 0x1A)]
    [ElementID(Game.YLAD, 0x19)]
    [ElementID(Game.LJ, 0x19)]
    [ElementID(Game.Gaiden, 0x19)]
    [ElementID(Game.Y8, 0x19)]
    public class DEElementSE : NodeElement
    {
        public enum Versions
        {
            Unknown,
            Y7AndBelow,
            LJAndAbove
        }

        public int SEVer;
       
        public byte SoundIndex;
        public byte Unk; //128 on fighter sounds
        public ushort CueSheet;

        public uint Flags;
        public float CustomDecayNearDist = 4;
        public float CustomDecayNearVol = 1;
        public float CustomDecayFarDist = 40;
        public float CustomDecayFarVol = 0.01f;
        public int Volume;

        //Version 2
        private int Unk1 = -1;
        private float Unk2 = 1;


        public DEElementSE()
        {
            Category = AuthNodeCategory.Element;
        }

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            if(CMN.VersionGreater(version, GameVersion.Yakuza6Demo))
                SEVer = reader.ReadInt32();
           
            SoundIndex = reader.ReadByte();
            Unk = reader.ReadByte();
            CueSheet = reader.ReadUInt16();

            Flags = reader.ReadUInt32();
            CustomDecayNearDist = reader.ReadSingle();
            CustomDecayNearVol = reader.ReadSingle();
            CustomDecayFarDist = reader.ReadSingle();
            CustomDecayFarVol = reader.ReadSingle();
            Volume = reader.ReadInt32();

            //LJ and above
            if (SEVer == 2)
            {
                Unk1 = reader.ReadInt32();
                Unk2 = reader.ReadSingle();
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            bool isVer1 = CMN.LastHActDEGame < Game.LJ;

            if (CMN.VersionGreater(version, GameVersion.Yakuza6Demo))
            {
                if (isVer1)
                    writer.Write(1);
                else
                    writer.Write(2);
            }
            
            writer.Write(SoundIndex);
            writer.Write(Unk);
            writer.Write(CueSheet);
            
            writer.Write(Flags);
            writer.Write(CustomDecayNearDist);
            writer.Write(CustomDecayNearVol);
            writer.Write(CustomDecayFarDist);
            writer.Write(CustomDecayFarVol);
            writer.Write(Volume);

            if (!isVer1)
            {
                writer.Write(Unk1);
                writer.Write(Unk2);
            }
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 48;
        }
    }
}
