using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
    [ElementID(Game.LAD7Gaiden, 0x19)]
    [ElementID(Game.LADIW, 0x19)]
    [ElementID(Game.LADPYIH, 0x19)]
    public class DEElementSE : NodeElement
    {
        public enum Versions
        {
            Unknown,
            Y7AndBelow,
            LJAndAbove
        }

        public int SEVer;
       
        public ushort SoundIndex;
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

            ushort soundIndexUnfiltered = reader.ReadUInt16();
            SoundIndex = (ushort)(soundIndexUnfiltered & ~(1 << 15));

            Unk = (byte)((soundIndexUnfiltered >> 15) & 1);
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

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            bool isVer1 = CMN.LastHActDEGame < Game.LJ;

            if (CMN.VersionGreater(version, GameVersion.Yakuza6Demo))
            {
                if (isVer1)
                    writer.Write(1);
                else
                    writer.Write(2);
            }

            ushort soundIndexVal = SoundIndex;

            if (Unk > 0)
                soundIndexVal |= 1 << 15;

            writer.Write(soundIndexVal);
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

        public static bool IsSpecialCuesheet(ushort cuesheet, Game game)
        {
            Type type = GetSpecialSoundTypeForGame(game);

            if (type == null)
                return false;

            return Enum.IsDefined(type, cuesheet);
        }
        public static Type GetSoundCuesheetTypeForGame(Game game)
        {
            switch(game)
            {
                default:
                    return null;
                case Game.Y6:
                    return typeof(CuesheetsY6);
                case Game.YK2:
                    return typeof(CuesheetsYK2);
                case Game.JE:
                    return typeof(CuesheetsJE);
                case Game.YLAD:
                    return typeof(CuesheetsYLAD);
                case Game.LJ:
                    return typeof(CuesheetsLJ);
                case Game.LAD7Gaiden:
                    return typeof(CuesheetsTMWEHI);
                case Game.LADIW:
                    return typeof(CuesheetsIW);
                case Game.LADPYIH:
                    return typeof(CuesheetsPYIH);
            }
        }

        public static Type GetSpecialSoundTypeForGame(Game game)
        {
            if (game < Game.YLAD)
                return typeof(SpecialSoundsY6);

            if (game == Game.YLAD || game == Game.LJ)
                return typeof(SpecialSoundsYLAD);

            if (game == Game.LAD7Gaiden)
                return typeof(SpecialSoundsTMWEHI);

            if (game == Game.LADIW || game == Game.LADPYIH)
                return typeof(SpecialSoundsIW);


            return typeof(SpecialSoundsTMWEHI);
        }
    }
}
