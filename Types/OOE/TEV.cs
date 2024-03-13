using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;



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
            public int CameraCount3;
            ///<summary>Offset: 0x34</summary>
            public uint UnkPtr2; //seems to be the 4 bytes right before the string table

            ///<summary>Offset: 0x38</summary>
            public bool UseSoundACB;
            ///<summary>Offset: 0x3C</summary>
            public uint UnkPtr3; //Unknown blank area, 44 bytes,  not constantly 44 

            ///<summary>Offset: 0x40</summary>
            public int CameraCount;

            ///<summary>Offset: 0x44</summary>
            public int CameraCount2;

            ///<summary>Offset: 0x48</summary>
            public int CharacterCount;

            ///<summary>Offset: 0x4C</summary>
            public int CharacterCount2;

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

        public ObjectBase Root;


        public TEV()
        {
            TEVHeader.UnkRegion1 = new byte[32];
            TEVHeader.UnkRegion2 = new byte[28];

            TEVHeader.UnkRegion2[7] = 1;
            TEVHeader.UnkRegion2[11] = 1;
            TEVHeader.UnkRegion2[15] = 13;
            TEVHeader.UnkRegion2[19] = 14;
            TEVHeader.UnkRegion2[23] = 5;

            //TEVHeader.UnkCount3 = 1;
        }


        public ObjectBase[] AllObjects
        {
            get
            {
                List<ObjectBase> objects = new List<ObjectBase>();

                void Process(ObjectBase node)
                {
                        objects.Add(node);

                    foreach (ITEVObject child in node.Children)
                        if(child is ObjectBase)
                            Process(child as ObjectBase);
                }

                Process(Root);

                return objects.ToArray();
            }
        }

        public Set2[] AllSet2
        {
            get
            {
                List<Set2> set2 = new List<Set2>();

                foreach (ObjectBase obj in AllObjects)
                {
                    foreach (ITEVObject childObj in obj.Children)
                        if (childObj is Set2)
                            set2.Add(childObj as Set2);
                }


                return set2.ToArray();
            }
        }



        public EffectBase[] AllEffects
        {
            get
            {
                List<EffectBase> effects = new List<EffectBase>();

                foreach(ObjectBase obj in AllObjects)
                {
                    foreach (ITEVObject childObj in obj.Children)
                        if (childObj is EffectBase)
                            effects.Add(childObj as EffectBase);
                }


                return effects.ToArray();
            }
        }


        public Set2Element1019 GetHActEventByName(string name)
        {
            return AllSet2.Where(x => x.EffectID == EffectID.Special).Cast<Set2Element1019>().FirstOrDefault(x => x.Type1019 == name);
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
            File.WriteAllBytes(path, output.Stream.ToArray());
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
