﻿using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;
using Yarhl.FileFormat;
using Yarhl.IO.Serialization;
using HActLib.Internal;
using ParLibrary;
using System.Runtime.CompilerServices;


namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class CMNHeader
    {
        //Base information
        public uint Version { get; set; }
        public uint Flags { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public int NodeDrawNum { get; set; }

        //Pointers
        public uint CutInfoPointer { get; set; }
        public uint AuthPagePointer { get; set; }
        public uint DisableFrameInfoPointer { get; set; }
        public uint ResourceCutInfoPointer { get; set; }
        public uint SoundInfoPointer { get; set; }
        public uint NodeInfoPointer { get; set; }

        public float ChainCameraIn { get; set; } = -1;
        public float ChainCameraOut { get; set; } = 1;

        public int Type { get; set; }
        public GameTick SkipPointTick { get; set; } = new GameTick(0);

        [Yarhl.IO.Serialization.Attributes.BinaryString(FixedSize = 0x4, MaxSize = 0x4)]
        public string Padding { get; set; } = "JHR";
    }

    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class CMNPageHeader
    {
        public uint Count { get; set; }
        public uint Size { get; set; }

        [Yarhl.IO.Serialization.Attributes.BinaryString(FixedSize = 8, MaxSize = 8)]
        public string Padding { get; set; }
    }

    //TODO: Make it so the writing matches original 100%
    //(Instead of being very similiar enough to work without any noticeable bugs)
    public class CMNConverter : IConverter<CMN, BinaryFormat>
    {
        public BinaryFormat Convert(CMN cmn)
        {
            CMN.LastFile = AuthFile.CMN;

            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);

            //we will return to this later
            writer.WriteOfType(cmn.Header);

            //First write: cut info
            uint cutInfoPointer = (uint)writer.Stream.Position;

            #region Cut Info

            //exclude the unknown 3 frames on length (its how the hact does it)
            writer.Write(cmn.CutInfo.Length);
            writer.WriteTimes(0, 12);

            for (int i = 0; i < cmn.CutInfo.Length; i++)
                writer.Write(cmn.CutInfo[i]);

            #endregion

            foreach(var page in cmn.AuthPages)
            {
                if (page.IsTalkPage())
                {
                    if (page.Format <= 1 && CMN.LastGameVersion >= GameVersion.DE2)
                    {
                        if ((page.Flag & 0x40) == 0)
                            page.Flag |= 0x40;
                    }
                }
                else
                {
                    if (page.Format <= 1)
                        page.Flag = 0;
                }

                if (CMN.LastGameVersion <= GameVersion.Yakuza6)
                    page.Format = 0;
                else if (CMN.LastGameVersion == GameVersion.DE1)
                    page.Format = 1;
                else
                    page.Format = 2;
            }

            //Second write: auth page info
            uint authPagePointer = (uint)writer.Stream.Position;

            #region Page Info

            int format = AuthPage.GetFormatForGameVer(cmn.GameVersion);

            if (cmn.GameVersion > GameVersion.Yakuza6Demo)
            {

                writer.Write(cmn.AuthPages.Count);
                writer.Write(cmn.GetPageSizes());
                writer.WriteTimes(0, 8);


                foreach (AuthPage page in cmn.AuthPages)
                {
                    if (format > 0)
                    {
                        writer.Write(page.Version);
                        writer.Write(page.Flag);
                    }

                    writer.Write(page.Start.Tick);
                    writer.Write(page.End.Tick);

                    if (format < 1)
                        writer.Write(page.Unk);

                    writer.Write(page.Transitions.Count);
                    writer.Write(page.GetTransitionSize());
                    writer.Write(page.SkipTick.Tick);

                    if(format > 0)
                        writer.Write(page.PageIndex);

                    writer.Write(page.SkipLinkIndexNum);
                    writer.WriteTimes(0, format > 0 ? 12 : 4);

                    if (format > 1)
                        writer.Write(page.PageTitleText.ToLength(32), fixedSize: 32, nullTerminator: false, encoding: writer.DefaultEncoding);

                    foreach (int i in page.SkipLink)
                        writer.Write(i);

                    foreach (Transition trans in page.Transitions)
                    {
                        writer.Write(trans.DestinationPageIndex);
                        writer.Write(trans.Conditions.Count);
                        writer.Write(trans.GetConditionSize());
                        writer.WriteTimes(0, 4);

                        foreach (Condition cond in trans.Conditions)
                        {

                            writer.Write(cond.ConditionID);
                            writer.Write(cond.Size());

                            writer.WriteTimes(0, 8);

                            cond.Write(writer);
                        }
                    }

                    if (page.IsTalkPage())
                    {
                        if (page.TalkInfo == null)
                        {
                            page.TalkInfo = new TalkInfo[0];
                            page.TalkInfoHeader = new TalkInfoHeader();
                        }

                        if (page.TalkInfo != null)
                        {
                            writer.Write(page.TalkInfo.Length);
                            writer.Write(page.TalkInfo.Length * 16);
                            writer.Write(page.TalkInfoHeader.Flags);
                            writer.WriteTimes(0, 4);

                            foreach (TalkInfo inf in page.TalkInfo)
                            {
                                writer.Write(inf.StartTick);
                                writer.Write(inf.EndTick);
                                writer.Write(inf.Flag);
                                writer.WriteTimes(0, 4);
                            }

                        }
                    }
                }

                #endregion
            }
            else
            {
                writer.Write(cmn.AuthPageUnk);
            }

            //Third write: disable frame info
            uint disableFramePointer = (uint)writer.Stream.Position;

            #region Disable Frame Info

            writer.Write(cmn.DisableFrameInfo.Count);
            writer.WriteTimes(0, 12);

            foreach (DisableFrameInfo inf in cmn.DisableFrameInfo)
                writer.WriteOfType(inf);

            #endregion

            //Fourth write: resource cut info
            uint resourceCutInfo = (uint)writer.Stream.Position;

            #region Resource Cut Info

            writer.Write(cmn.ResourceCutInfo.Length);
            writer.WriteTimes(0, 12);

            //start end probably not single
            foreach (float f in cmn.ResourceCutInfo)
                writer.Write(f);

            #endregion

            uint soundInfo = (uint)writer.Stream.Position;

            #region Sound Info

            writer.Write(cmn.SoundInfo.Length);
            writer.WriteTimes(0, 12);

            foreach (float f in cmn.SoundInfo)
                writer.Write(f);

            #endregion

            uint nodeInfoPointer = (uint)writer.Stream.Position;

            #region Node Info

            if (cmn.Root == null)
                throw new Exception("A HAct CMN has to have atleast one root node!");

            cmn.Root.Write(writer, cmn.GameVersion, cmn.Header.Version);

            #endregion

            CMNHeader newHeader = cmn.Header.Copy();
            newHeader.CutInfoPointer = cutInfoPointer;
            newHeader.AuthPagePointer = authPagePointer;
            newHeader.DisableFrameInfoPointer = disableFramePointer;
            newHeader.ResourceCutInfoPointer = resourceCutInfo;
            newHeader.SoundInfoPointer = soundInfo;
            newHeader.NodeInfoPointer = nodeInfoPointer;

            newHeader.Padding = "JHR";

            writer.Stream.Seek(0, SeekMode.Start);
            writer.WriteOfType(newHeader);

            return binary;
        }
    }

    public class CMN : BaseCMN
    {
        public GameVersion GameVersion = GameVersion.Yakuza6;

        public CMNHeader Header = new CMNHeader();
        public CMNPageHeader AuthPageHeader = new CMNPageHeader();

        public List<AuthPage> AuthPages = new List<AuthPage>();
        public byte[] AuthPageUnk = new byte[0]; //Yakuza 6 only

        public float[] SoundInfo = new float[0];

        //messy but it is what it is
        internal static AuthFile LastFile;
        internal static GameVersion LastGameVersion;
        public static Game LastHActDEGame;


        public static CMN Read(string path, Game game)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path), game);
        }

        public static DataStream WriteToStream(CMN cmn)
        {
            if (cmn == null)
                return null;

            LastGameVersion = cmn.GameVersion;
            LastFile = AuthFile.CMN;

            BinaryFormat output = (BinaryFormat)ConvertFormat.With<CMNConverter>(cmn);

            return output.Stream;
        }

        public static void Write(CMN cmn, string path)
        {
            if (cmn == null)
                return;

            DataStream file = WriteToStream(cmn);

            LastGameVersion = cmn.GameVersion;
            LastFile = AuthFile.CMN;

            BinaryFormat output = (BinaryFormat)ConvertFormat.With<CMNConverter>(cmn);
            File.WriteAllBytes(path, output.Stream.ToArray());
        }


        public static bool VersionGreater(GameVersion ver, GameVersion comparison)
        {
            return (uint)ver > (uint)comparison;
        }

        public static bool VersionEqualsGreater(GameVersion ver, GameVersion comparison)
        {
            return (uint)ver >= (uint)comparison;
        }

        public static bool VersionEqualsLess(GameVersion ver, GameVersion comparison)
        {
            return (uint)ver <= (uint)comparison;
        }

        public static bool GameEqualsGreater(Game ver, Game comparison)
        {
            return (uint)ver >= (uint)comparison;
        }

        public static bool GameEqualsLess(Game ver, Game comparison)
        {
            return (uint)ver <= (uint)comparison;
        }

        public static bool IsOE(GameVersion version)
        {
            if (version == GameVersion.Y0_K1)
                return true;
            else
                return false;
        }

        public static OECMN ToOE(CMN cmn, uint targetOEVer)
        {
            if (targetOEVer < 10)
                targetOEVer = 10;
            if (targetOEVer > 16)
                targetOEVer = 16;

            LastGameVersion = GameVersion.Y0_K1;
            OECMN converted = (OECMN)ConvertFormat.With<DE2OECmn>(new DEToOEConversionInfo() { Cmn = cmn, TargetVer = targetOEVer });
            return converted;
        }

        public static bool IsDE(GameVersion version)
        {
            if (version == GameVersion.OOE || version == GameVersion.OOE_KENZAN)
                return false;

            return !IsOE(version);
        }

        public static bool IsOEGame(Game game)
        {
            if (game == Game.Y5 ||
                game == Game.Ishin ||
                game == Game.Y0 ||
                game == Game.YK1)
                return true;
            else
                return false;
        }

        public static bool IsDEGame(Game game)
        {
            if (game == Game.Y6Demo ||
                game == Game.Y6 ||
                game == Game.YK2 ||
                game == Game.JE ||
                game == Game.YLAD ||
                game == Game.LJ ||
                game == Game.LADIW ||
                game == Game.LAD7Gaiden)
                return true;
            else
                return false;
        }

        public static Game[] GetOEGames()
        {
            List<Game> games = new List<Game>();

            for (int i = 0; i < Enum.GetValues<Game>().Length; i++)
                if (IsOEGame((Game)i))
                    games.Add((Game)i);

            return games.ToArray();
        }

        public static Game[] GetDEGames()
        {
            List<Game> games = new List<Game>();

            for (int i = 0; i < Enum.GetValues<Game>().Length; i++)
                if (IsDEGame((Game)i))
                    games.Add((Game)i);

            return games.ToArray();
        }

        public static Game GetGameFromString(string str)
        {
            switch (str.ToLowerInvariant())
            {
                case "y3":
                    return Game.Y3;
                case "y4":
                    return Game.Y4;

                case "y5":
                    return Game.Y5;
                case "yakuza5":
                    return Game.Y5;

                case "ishin":
                    return Game.Ishin;

                case "y0":
                    return Game.Y0;
                case "yakuza0":
                    return Game.Y0;

                case "yk1":
                    return Game.YK1;
                case "yakuzakiwami":
                    return Game.YK1;

                case "y6":
                    return Game.Y6;
                case "y6demo":
                    return Game.Y6Demo;
                case "yakuza6":
                    return Game.Y6;

                case "k2":
                    return Game.YK2;
                case "yk2":
                    return Game.YK2;
                case "yakuzakiwami2":
                    return Game.YK2;

                case "je":
                    return Game.JE;
                case "judge":
                    return Game.JE;

                case "ylad":
                    return Game.YLAD;
                case "y7":
                    return Game.YLAD;
                case "bestgame":
                    return Game.YLAD;

                case "lj":
                    return Game.LJ;

                case "gaiden":
                    return Game.LAD7Gaiden;

                case "y8":
                    return Game.LADIW;
            }


            return (Game)99999;
        }

        public static GameVersion GetVersionForGame(Game game)
        {
            switch (game)
            {
                default:
                    return GameVersion.DE2;

                case Game.Y3:
                    return GameVersion.OOE;
                case Game.Y4:
                    return GameVersion.OOE;
                case Game.Y5:
                    return GameVersion.Y0_K1;
                case Game.Ishin:
                    return GameVersion.Y0_K1;
                case Game.Y0:
                    return GameVersion.Y0_K1;
                case Game.YK1:
                    return GameVersion.Y0_K1;
                case Game.Y6Demo:
                    return GameVersion.Yakuza6Demo;
                case Game.Y6:
                    return GameVersion.Yakuza6;
                case Game.YK2:
                    return GameVersion.DE1;
                case Game.JE:
                    return GameVersion.DE1;
                case Game.YLAD:
                    return GameVersion.DE2;
                case Game.LJ:
                    return GameVersion.DE2;
                case Game.LAD7Gaiden:
                    return GameVersion.DE2;
                case Game.LADIW:
                    return GameVersion.DE2;
            }
        }


        public static CMN Read(byte[] buffer, Game game)
        {
            if (buffer == null || buffer.Length < 0)
                return null;

            if (!Reflection.Done)
                Reflection.Process();

            LastFile = AuthFile.CMN;

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader cmnReader = new DataReader(readStream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };

            GameVersion version = GetVersionForGame(game);

            //Only used reading nodes etc, never used in writing
            LastGameVersion = version;
            LastHActDEGame = game;

            CMN cmn = new CMN();
            cmn.GameVersion = LastGameVersion;

            //Reader header
            cmn.Header = cmnReader.Read<CMNHeader>();
            long HeaderEndPosition = cmnReader.Stream.Position;

            if (cmn.Header.Version < 18)
                throw new Exception("Not a Dragon Engine HAct");

            //Read sound
            cmnReader.Stream.RunInPosition(delegate
            {
                int count = cmnReader.ReadInt32();

                cmn.SoundInfo = new float[count];
                cmnReader.ReadBytes(12);

                for (int i = 0; i < count; i++)
                    cmn.SoundInfo[i] = cmnReader.ReadSingle();

            }, cmn.Header.SoundInfoPointer, SeekMode.Start);

            //Read cut info
            cmnReader.Stream.RunInPosition(delegate
            {
                int count = cmnReader.ReadInt32();
                cmnReader.ReadBytes(12);

                cmn.CutInfo = new float[count];

                for (int i = 0; i < count; i++)
                    cmn.CutInfo[i] = cmnReader.ReadSingle();

            }, cmn.Header.CutInfoPointer, SeekMode.Start);

            cmnReader.Stream.RunInPosition(delegate
            {
                int count = cmnReader.ReadInt32();
                cmnReader.ReadBytes(12);

                cmn.ResourceCutInfo = new float[count];

                for (int i = 0; i < count; i++)
                    cmn.ResourceCutInfo[i] = cmnReader.ReadSingle();

            }, cmn.Header.ResourceCutInfoPointer, SeekMode.Start);

            cmnReader.Stream.RunInPosition(delegate
            {
                int count = cmnReader.ReadInt32();
                cmnReader.ReadBytes(12);

                cmn.DisableFrameInfo = new List<DisableFrameInfo>(new DisableFrameInfo[count]);

                for (int i = 0; i < count; i++)
                    cmn.DisableFrameInfo[i] = cmnReader.Read<DisableFrameInfo>();

            }, cmn.Header.DisableFrameInfoPointer, SeekMode.Start);

            cmn.ReadNodes(cmnReader);


            //Read auth pages
            if (VersionGreater(version, GameVersion.Yakuza6Demo))
                cmn.ReadAuthPages(cmnReader);
            else
            {
                cmnReader.Stream.Seek(cmn.Header.AuthPagePointer, SeekMode.Start);
                cmn.AuthPageUnk = cmnReader.ReadBytes((int)(cmn.Header.DisableFrameInfoPointer - cmnReader.Stream.Position));
            }

            return cmn;
        }

        internal void ReadAuthPages(DataReader reader)
        {
            reader.Stream.Seek(Header.AuthPagePointer, SeekMode.Start);
            AuthPageHeader = reader.Read<CMNPageHeader>();

            AuthPages = new List<AuthPage>(new AuthPage[AuthPageHeader.Count]);

#if !DEBUG
            try
            {
#endif
            for (int i = 0; i < AuthPages.Count; i++)
            {
                AuthPage page = (AuthPage)ConvertFormat.With<AuthPageConverter>(new BinaryFormat(reader.Stream));

                if (page.Format <= 1)
                    page.PageTitleText = "Page " + i;

                AuthPages[i] = page;
#if !DEBUG
                }
            }
            catch
            {
                Console.WriteLine("Error reading auth pages.");
                AuthPages = new List<AuthPage>();
#endif
            }
        }

        internal void ReadNodes(DataReader reader)
        {
            reader.Stream.Seek(Header.NodeInfoPointer);

            void ReadNode()
            {
                Root = (Node)ConvertFormat.With<DENodeConverter>(new NodeConvInf()
                {
                    format = new BinaryFormat(reader.Stream),
                    version = Header.Version,
                    gameVersion = LastGameVersion,
                    file = LastFile
                });
            }

            ReadNode();
        }

        //Get size of the pages. used for writing
        public int GetPageSizes()
        {
            int size = 0;

            foreach (AuthPage page in AuthPages)
                size += page.GetPageSize();

            return size;
        }


        public Node[] AllNodes
        {
            get
            {
                List<Node> nodes = new List<Node>();

                void Process(Node node)
                {
                    nodes.Add(node);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return nodes.ToArray();
            }

            set
            {
                //It's assumed the root node contains everything 
                Root = value[0];
            }
        }

        public NodeElement[] AllElements
        {
            get
            {
                List<NodeElement> elements = new List<NodeElement>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Element)
                        elements.Add(node as NodeElement);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return elements.ToArray();
            }
        }

        public NodeCamera[] AllCameras
        {
            get
            {
                List<NodeCamera> cameras = new List<NodeCamera>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Camera)
                        cameras.Add(node as NodeCamera);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return cameras.ToArray();
            }
        }

        public DENodeCharacter[] AllCharacters
        {
            get
            {
                List<DENodeCharacter> chars = new List<DENodeCharacter>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Character)
                        chars.Add(node as DENodeCharacter);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return chars.ToArray();
            }
        }

        public override uint Version { get { return Header.Version; } set { Header.Version = value; } }
        public override float HActStart { get { return Header.Start; } set { Header.Start = value; } }
        public override float HActEnd { get { return Header.End; } set { Header.End = value; } }

        public override float GetChainCameraIn()
        {
            return Header.ChainCameraIn;
        }

        public override void SetChainCameraIn(float val)
        {
            Header.ChainCameraIn = val;
        }

        public override float GetChainCameraOut()
        {
            return Header.ChainCameraOut;
        }

        public override void SetChainCameraOut(float val)
        {
            Header.ChainCameraOut = val;
        }

        public override uint GetFlags()
        {
            return Header.Flags;
        }

        public override void SetFlags(uint flags)
        {
            Header.Flags = flags;
        }

        public override int GetNodeDrawNum()
        {
            return Header.NodeDrawNum;
        }

        public override void SetNodeDrawNum(int num)
        {
            Header.NodeDrawNum = num;
        }

        public override Node[] GetNodes()
        {
            return AllNodes;
        }
    }
}
