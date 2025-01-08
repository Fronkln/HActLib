using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.Internal;
using Yarhl.FileSystem;

namespace HActLib
{
    public class OECMN : BaseCMN
    {
        [Yarhl.IO.Serialization.Attributes.Serializable]
        public class Header
        {
            //Base information
            public uint Version { get; set; }
            public uint Flags { get; set; }
            public float Start { get; set; }
            public float End { get; set; }
            public int NodeDrawNum { get; set; }

            //Pointers
            public uint CutInfoPointer { get; set; }
            public uint DisableFrameInfoPointer { get; set; }
            public uint ResourceCutInfoPointer { get; set; }
            public uint SoundInfoPointer { get; set; } // ver > 10
            public uint NodeInfoPointer { get; set; }

            public float ChainCameraIn { get; set; }
            public float ChainCameraOut { get; set; }

        }

        public Header CMNHeader = new Header();
        public uint[] SoundInfo = new uint[0];

        public static OECMN Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static OECMN Read(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 0)
                return null;


            if (!Reflection.Done)
                Reflection.Process();

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader cmnReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            OECMN cmn = new OECMN();
            cmn.CMNHeader = new Header();


            cmn.CMNHeader.Version = cmnReader.ReadUInt32();
            cmn.CMNHeader.Flags = cmnReader.ReadUInt32();
            cmn.CMNHeader.Start = cmnReader.ReadSingle();
            cmn.CMNHeader.End = cmnReader.ReadSingle();
            cmn.CMNHeader.NodeDrawNum = cmnReader.ReadInt32();

            int cutInfoPtr = 0;
            int disableFrameInfoPtr = 0;
            int resourceCutInfoPtr = 0;
            int soundInfoPtr = 0;
            int nodeInfoPtr = 0;

            if(cmn.CMNHeader.Version <= 10)
            {
                cutInfoPtr = cmnReader.ReadInt32();
                resourceCutInfoPtr = cmnReader.ReadInt32();
                soundInfoPtr = cmnReader.ReadInt32();
                nodeInfoPtr = cmnReader.ReadInt32();
            }
            else
            {
                cutInfoPtr = cmnReader.ReadInt32();
                disableFrameInfoPtr = cmnReader.ReadInt32();
                resourceCutInfoPtr = cmnReader.ReadInt32();
                soundInfoPtr = cmnReader.ReadInt32();
                nodeInfoPtr = cmnReader.ReadInt32();
            }

            cmn.CMNHeader.ChainCameraIn = cmnReader.ReadSingle();
            cmn.CMNHeader.ChainCameraOut = cmnReader.ReadSingle();

            CMN.LastGameVersion = GameVersion.Y0_K1;

            switch (cmn.CMNHeader.Version)
            {
                default:
                    throw new Exception("Unknown version: " + cmn.CMNHeader.Version);
                case 10:
                    CMN.LastHActDEGame = Game.Y5;
                    break;
                case 15:
                    CMN.LastHActDEGame = Game.Ishin;
                    break;
                case 16:
                    CMN.LastHActDEGame = Game.Y0;
                    break;
            }


            if (soundInfoPtr > 0)
            {
                //Read sound
                cmnReader.Stream.RunInPosition(delegate
                {
                    int count = cmnReader.ReadInt32();

                    cmn.SoundInfo = new uint[count];
                    cmnReader.ReadBytes(12);

                    for (int i = 0; i < count; i++)
                        cmn.SoundInfo[i] = cmnReader.ReadUInt32();

                }, soundInfoPtr, SeekMode.Start);
            }

            //Read cut info
            if (cutInfoPtr > 0)
            {
                cmnReader.Stream.RunInPosition(delegate
                {
                    int count = cmnReader.ReadInt32();
                    cmnReader.ReadBytes(12);

                    cmn.CutInfo = new float[count];

                    for (int i = 0; i < count; i++)
                        cmn.CutInfo[i] = cmnReader.ReadSingle();

                }, cutInfoPtr, SeekMode.Start);
            }

            if (resourceCutInfoPtr > 0)
            {
                cmnReader.Stream.RunInPosition(delegate
                {
                    int count = cmnReader.ReadInt32();
                    cmnReader.ReadBytes(12);

                    cmn.ResourceCutInfo = new float[count];

                    for (int i = 0; i < count; i++)
                        cmn.ResourceCutInfo[i] = cmnReader.ReadSingle();

                }, resourceCutInfoPtr, SeekMode.Start);
            }

            if (disableFrameInfoPtr > 0)
            {
                cmnReader.Stream.RunInPosition(delegate
                {
                    int count = cmnReader.ReadInt32();
                    cmnReader.ReadBytes(12);

                    cmn.DisableFrameInfo = new List<DisableFrameInfo>(new DisableFrameInfo[count]);

                    for (int i = 0; i < count; i++)
                    {
                        if (cmn.CMNHeader.Version > 10)
                            cmn.DisableFrameInfo[i] = cmnReader.Read<DisableFrameInfo>();
                        else
                            cmn.DisableFrameInfo[i] = new DisableFrameInfo() { Start = cmnReader.ReadSingle() };
                    }

                }, disableFrameInfoPtr, SeekMode.Start);
            }

            cmnReader.Stream.Seek(nodeInfoPtr, SeekMode.Start);
            cmn.ReadNodes(cmnReader);

            return cmn;
        }

        public static DataStream WriteToStream(OECMN cmn)
        {
            if (cmn == null)
                return null;

            CMN.LastGameVersion = GameVersion.Y0_K1;
            BinaryFormat output = (BinaryFormat)ConvertFormat.With<OECMNConverter>(cmn);

            return output.Stream;
        }

        public static void Write(OECMN cmn, string path)
        {
            if (cmn == null)
                return;

            File.WriteAllBytes(path, WriteToStream(cmn).ToArray());
        }


        internal void ReadNodes(DataReader reader)
        {

            Root = (Node)ConvertFormat.With<OENodeConverter>(new NodeConvInf()
            {
                format = new BinaryFormat(reader.Stream),
                version = CMNHeader.Version,
                gameVersion = GameVersion.Y0_K1,
            });
        }

        public static CMN ToDE(OECMN cmn, GameVersion target)
        {
            if (!CMN.IsDE(target))
                target = GameVersion.DE2;

            CMN.LastGameVersion = target;
            CMN converted = (CMN)ConvertFormat.With<OE2DECmn>(cmn);
            return converted;
        }

        public static uint GetCMNVersionForGame(Game game)
        {
            switch (game)
            {
                default:
                    return 18;

                case Game.Y5:
                    return 10;
                case Game.Y0:
                    return 16;
                case Game.YK1:
                    return 16;
                case Game.Ishin:
                    return 15;
            }
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

        public OENodeCharacter[] AllCharacters
        {
            get
            {
                List<OENodeCharacter> chars = new List<OENodeCharacter>();

                void Process(Node node)
                {
                    if (node.Category == AuthNodeCategory.Character)
                        chars.Add(node as OENodeCharacter);

                    foreach (Node child in node.Children)
                        Process(child);
                }

                Process(Root);

                return chars.ToArray();
            }
        }


        public override uint Version { get { return CMNHeader.Version; } set { CMNHeader.Version = value; } }
        public override float HActStart { get { return CMNHeader.Start; } set { CMNHeader.Start = value; } }
        public override float HActEnd { get { return CMNHeader.End; } set { CMNHeader.End = value; } }

        public override float GetChainCameraIn()
        {
            return CMNHeader.ChainCameraIn;
        }

        public override void SetChainCameraIn(float val)
        {
            CMNHeader.ChainCameraIn = val;
        }

        public override float GetChainCameraOut()
        {
            return CMNHeader.ChainCameraOut;
        }

        public override void SetChainCameraOut(float val)
        {
            CMNHeader.ChainCameraOut = val;
        }

        public override uint GetFlags()
        {
            return CMNHeader.Flags;
        }

        public override void SetFlags(uint flags)
        {
            CMNHeader.Flags = flags;
        }

        public override int GetNodeDrawNum()
        {
            return CMNHeader.NodeDrawNum;
        }

        public override void SetNodeDrawNum(int num)
        {
            CMNHeader.NodeDrawNum = num;
        }

        public override Node[] GetNodes()
        {
            return AllNodes;
        }
    }
}

