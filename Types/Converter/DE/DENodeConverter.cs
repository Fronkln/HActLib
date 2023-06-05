using System;
using System.Collections.Generic;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.Internal;

namespace HActLib
{
    public class DENodeConverter : IConverter<NodeConvInf, Node>
    {
        //DE Only refer to OENodeConverter for OE
        public Node Convert(NodeConvInf inf)
        {
            DataReader reader = new DataReader(inf.format.Stream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };
            long startPos = reader.Stream.Position;

            AuthNodeCategory category = 0;
            uint elementKind = 0;

            if (inf.file == AuthFile.CMN)
            {
                reader.Stream.RunInPosition(delegate 
                { 
                    category = (AuthNodeCategory)reader.ReadUInt32();

                    if (category == AuthNodeCategory.Element)
                    {
                        reader.Stream.Seek(60, SeekMode.Current);
                        elementKind = reader.ReadUInt32();
                    }

                }, 16, SeekMode.Current);
            }
            else if(inf.file == AuthFile.BEP)
            {
                reader.Stream.RunInPosition(delegate
                {
                    category = (AuthNodeCategory)reader.ReadUInt16();

                    if (category == AuthNodeCategory.Element)
                        elementKind = reader.ReadUInt16();

                }, 64, SeekMode.Current);
            }

            Node node;

            switch (category)
            {
                default:
                    //Console.WriteLine("Don't know how to handle " + category);
                    node = new Node();
                    break;
                case AuthNodeCategory.Path:
                    node = new DENodePath();
                    break;
                case AuthNodeCategory.Camera:
                    node = new NodeCamera();
                    break;

                case AuthNodeCategory.CameraMotion:
                    node = new NodeCameraMotion();
                    break;

                case AuthNodeCategory.CharacterMotion:
                    node = new DENodeCharacterMotion();
                    break;
                  
                case AuthNodeCategory.Character:
                    node = new DENodeCharacter();
                    break;
                    
                case AuthNodeCategory.Model_node:
                    node = new NodeModel();
                    break;

                case AuthNodeCategory.Asset:
                    node = new NodeAsset();
                    break;

                //case AuthNodeCategory.FolderCondition:
                 //   node = new DENodeConditionFolder();
                 //   break;

                case AuthNodeCategory.CharacterBehavior:
                    node = new DENodeCharacterBehavior();
                    break;

                case AuthNodeCategory.Element:
                    //Does the current ID have a element linked to it
                    if (Reflection.ElementNodes[CMN.LastHActDEGame].ContainsKey(elementKind))
                        node = (NodeElement)Activator.CreateInstance(Reflection.ElementNodes[CMN.LastHActDEGame][elementKind]);
                    else
                        node = new NodeElement();
                    break;


                case AuthNodeCategory.ModelMotion:
                    node = new NodeMotionBase();
                    break;

            }

            node.ReadBaseInfo(reader, inf);

            //I don't know how, i don't want to know how. But BEP node size is misleading.
            //Every single fucking element's data has this value that has ranges between 2-3
            //But it's not accounted for in the node size, why JUST WHY GODDAMN IT
            //It's clearly part of the element's core information. So why trick me you sick fuck 
            int nodeSize = (inf.file == AuthFile.CMN ?  node.NodeSize * 4 : node.NodeSize);
            long preReadPos = reader.Stream.Position;
            long postDataPos = preReadPos + nodeSize;

            inf.expectedSize = nodeSize;
            node.ReadNodeData(reader, inf, CMN.LastGameVersion);
            long postReadPos = reader.Stream.Position;


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
            else if(postReadPos > postDataPos)
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
