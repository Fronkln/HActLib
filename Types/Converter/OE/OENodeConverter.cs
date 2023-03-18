using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.Internal;

namespace HActLib
{
    public class OENodeConverter : IConverter<NodeConvInf, Node>
    {
        public Node Convert(NodeConvInf inf)
        {
            DataReader reader = new DataReader(inf.format.Stream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };
            long startPos = reader.Stream.Position;

            AuthNodeCategory category = 0;
            reader.Stream.RunInPosition(delegate { category = (AuthNodeCategory)reader.ReadUInt32(); }, 16, SeekMode.Current);

            //HEUTERISTIC BASED ON MY OWN OBSERVATION
            if (category == AuthNodeCategory.Model_node)
                category = AuthNodeCategory.Element;
            else if (category == AuthNodeCategory.Motion_model)
                category = AuthNodeCategory.Model_node;
            else if (category == AuthNodeCategory.InstanceMotionData)
                category = AuthNodeCategory.FolderCondition;
         
            uint elementKind = 0;

            if (category == AuthNodeCategory.Element)
            {
                long pos = reader.Stream.Position;
                if(inf.version > 10)
                    reader.Stream.RunInPosition(delegate { elementKind = reader.ReadUInt32(); }, 80, SeekMode.Current);
                else
                    reader.Stream.RunInPosition(delegate { elementKind = reader.ReadUInt32(); }, 64, SeekMode.Current);

                reader.Stream.Position = pos;
            }
            
            Node node = null;

            switch (category)
            {
                default:
                    node = new Node();
                    break;
                case AuthNodeCategory.Path:
                    node = new NodePathBase();
                    break;

                case AuthNodeCategory.Camera:
                        node = new NodeCamera();
                    break;

                case AuthNodeCategory.Character:
                        node = new OENodeCharacter();
                    break;

                case AuthNodeCategory.CharacterMotion:
                    node = new NodeMotionBase();
                    break;

                case AuthNodeCategory.CameraMotion:
                        node = new NodeCameraMotion();
                    break;

                case AuthNodeCategory.Model_node:
                    node = new NodeModel();
                    break;

                case AuthNodeCategory.Element:
                    if (Reflection.ElementNodes[CMN.LastHActDEGame].ContainsKey(elementKind))
                        node = (NodeElement)Activator.CreateInstance(Reflection.ElementNodes[CMN.LastHActDEGame][elementKind]);
                    else
                        node = new NodeElement();// new NodeElement();
                    break;
            }


            node.ReadBaseInfo(reader, inf);

            int nodeSize = (inf.file == AuthFile.CMN ? node.NodeSize * 4 : node.NodeSize);
            long preReadPos = reader.Stream.Position;
            long postDataPos = preReadPos + nodeSize;

            node.ReadNodeData(reader, inf, CMN.LastGameVersion);

            long postReadPos = reader.Stream.Position;

            node.Category = category; //just to make OE happy

            System.Diagnostics.Debug.Print("Read: " + node.ToString() + " " + node.Category + " " + node.Name);

            if (postDataPos > postReadPos)
            {
                string debugStr = $"[WARNING] Read {postDataPos - postReadPos} bytes less than the expected {nodeSize}!\nAddress:{preReadPos}\nGNode type: {node.Category}\nUID:{node.Guid}\nName:{node.Name}";

                if (node.Category == AuthNodeCategory.Element)
                    debugStr += $"\nElement Kind: {(node as NodeElement).ElementKind}\nElement Name: {Reflection.GetElementNameByID((node as NodeElement).ElementKind, CMN.LastHActDEGame)}\nElement Version: {(node as NodeElement).Version}";

                debugStr += "\n";
                System.Diagnostics.Debug.WriteLine(debugStr);

                node.unkBytes = reader.ReadBytes((int)(postDataPos - postReadPos));
            }
            else if (postReadPos > postDataPos)
            {
                string debugStr = $"[WARNING] Read {(postReadPos - postDataPos)} bytes more ({postReadPos - preReadPos}) than the expected {nodeSize}!\nAddress:{preReadPos}\nGNode type: {node.Category}\nGUID:{node.Guid}";

                if (node.Category == AuthNodeCategory.Element)
                    debugStr += $"\nElement Kind: {(node as NodeElement).ElementKind}\nElement Name: {Reflection.GetElementNameByID((node as NodeElement).ElementKind, CMN.LastHActDEGame)}\nElement Version: {(node as NodeElement).Version}";

                debugStr += "\n";
                System.Diagnostics.Debug.WriteLine(debugStr);

                reader.Stream.Seek(postDataPos, SeekMode.Start);
            }


            for (int i = 0; i < node.ChildCount; i++)
            {
                Node child = Convert(inf);
                child.Parent = node;

                node.Children.Add(child);
            }


            return node;
        }
    }
}
