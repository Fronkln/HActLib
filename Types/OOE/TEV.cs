using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;
using System.Security.AccessControl;

namespace HActLib
{
    public class TEV
    {
        public struct Header
        {
            public string Magic;
            public uint Version;
            public uint FormatDate;
            public uint Unk1;

            ///<summary>Offset: 0x28</summary>
            public uint UnkCount1;
            ///<summary>Offset: 0x2C</summary>
            public uint DataPtr2;

            ///<summary>Offset: 0x30</summary>
            public uint UnkCount2;
            ///<summary>Offset: 0x34</summary>
            public uint UnkPtr2; //seems to be the 4 bytes right before the string table

            ///<summary>Offset: 0x38</summary>
            public uint UnkCount3;
            ///<summary>Offset: 0x3C</summary>
            public uint UnkPtr3; //Unknown blank area, 44 bytes,  not constantly 44 

            ///<summary>Offset: 0x40</summary>
            public uint UnkVal1;

            ///<summary>Offset: 0x44</summary>
            public uint UnkVal2;

            ///<summary>Offset: 0x48</summary>
            public uint UnkVal3;

            ///<summary>Offset: 0x4C</summary>
            public uint UnkVal4;

            //32 bytes, mostly all zeroes, not the case on 6090 hact though.
            public byte[] UnkRegion1;

            //28 bytes, random numbers. followed by the final value which is string table pointer
            public byte[] UnkRegion2;

            //Amount of padding between data ptr 1 and data ptr 2
            //Because i couldn't figure out the magic behind them, LOL!!!
            public int DataPadding;

            //Mysterious padding that unk3 pointer leads to
            public int StringTableOffset;
        }

        public Header TEVHeader = new Header();

        /// <summary>
        /// ID of the cuesheet the HAct uses.
        /// </summary>
        public uint CuesheetID;

        public ObjectBase[] Objects;
        public Set2[] Set2;
        public EffectBase[] Effects;

        internal Dictionary<int, ObjectBase> PointerSet1 = new Dictionary<int, ObjectBase>();
        internal Dictionary<int, Set2> PointerSet2 = new Dictionary<int, Set2>();
        internal Dictionary<int, EffectBase> PointerSet3 = new Dictionary<int, EffectBase>();

        public Set2Element1019 GetHActEventByName(string name)
        {
            return Set2.Where(x => x.EffectID == EffectID.Special).Cast<Set2Element1019>().FirstOrDefault(x => x.Type1019 == name);
        }

        public static TEV Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static TEV Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader cmnReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            TEV tev = (TEV)ConvertFormat.With<TEVReader>(new BinaryFormat(cmnReader.Stream));

            return tev;
        }

        public static void Write(TEV tev, string path)
        {
            if (tev == null)
                return;

            CMN.LastGameVersion = GameVersion.OOE;

            BinaryFormat output = (BinaryFormat)ConvertFormat.With<TEVWriter>(tev);
            output.Stream.WriteTo(path);
        }

        public static OECMN ToOE(TEV tev, CSVHAct csvData, uint targetOEVer)
        {
            if (targetOEVer < 10)
                targetOEVer = 10;
            if (targetOEVer > 16)
                targetOEVer = 16;

            CMN.LastGameVersion = GameVersion.Y0_K1;
            OECMN converted = (OECMN)ConvertFormat.With<OOEToOE>(new OOEToOEConversionInfo() { Tev = tev, CsvData = csvData, TargetVer = targetOEVer });
            
            return converted;
        }

        public static CMN ToDE(TEV tev, Game game, CSVHAct csvData)
        {
            if (!CMN.IsDEGame(game))
                game = Game.YK2;

            CMN.LastGameVersion = CMN.GetVersionForGame(game);
            CMN.LastHActDEGame = game;

            CMN cmn = (CMN)ConvertFormat.With<OOEToDE>(new OOEToDEConversionInfo() { Tev = tev, CsvData = csvData});

            if (!CMN.IsDEGame(game))
                game = Game.YK2;

            cmn.GameVersion = CMN.LastGameVersion;

            return cmn;
        }
    }
}
