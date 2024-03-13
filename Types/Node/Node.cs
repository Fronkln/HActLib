using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Yarhl.IO;

namespace HActLib
{

    [Serializable]
    [DebuggerDisplay("{Name}")]
    public class Node 
    {
        /// <summary> Data exclusive to BEP files. </summary>
        public BEPData BEPDat = new BEPData();

        public Guid Guid = new Guid();

        public AuthNodeCategory Category;
        internal int NodeSize; 
        internal int ChildCount; //none in bep
        public uint Flag;

        #region CMN
        /// <summary> CMN only.</summary>
        public int Priority { get; set; }
        /// <summary> CMN Only. </summary>
        public string Name { get; set; } = "";
        /// <summary> CMN Only. </summary>
        public Node Parent;
        /// <summary> CMN Only. </summary>
        public List<Node> Children = new List<Node>();
        #endregion

        //bytes we didnt parse
        public byte[] unkBytes;

        public Node Clone()
        {
            return ObjectExtensions.Copy(this);
        }

        //Copy base information of node to another  (used in OE to DE conversions for example)
        public static void CopyBaseInfo(Node src, Node dest)
        {
            dest.Guid = src.Guid;
            dest.Category = src.Category;
            dest.Flag = src.Flag;
            dest.Priority = src.Priority;
            
            if(!string.IsNullOrEmpty(src.Name))
                dest.Name = src.Name;
            
            dest.Parent = src.Parent;
            dest.Children = src.Children;
            dest.unkBytes = src.unkBytes;
        }

        public static void CopyBaseElementInfo(NodeElement src, NodeElement dest)
        {
            dest.Start = src.Start;
            dest.End = src.End;
            dest.ElementFlag = src.ElementKind;
            dest.PlayType = src.PlayType;
            dest.Version = src.Version;
            dest.UpdateTimingMode = src.UpdateTimingMode;
        }

        internal void ReadBaseInfo(DataReader reader, NodeConvInf inf)
        {
            Guid = new Guid(reader.ReadBytes(16));

            if (inf.file == AuthFile.BEP)
            {
                BEPDat.Guid2 = new Guid(reader.ReadBytes(16));
                BEPDat.Bone = reader.Read<PXDHash>();
                Category = (AuthNodeCategory)reader.ReadUInt16();
                BEPDat.PropertyType = reader.ReadUInt16();

                if (Category == AuthNodeCategory.Element)
                    Name = Internal.Reflection.GetElementNameByID(BEPDat.PropertyType, CMN.LastHActDEGame).Replace("e_auth_element_", "").Replace("_", " ").ToTitleCase();
                else
                    Name = Category.ToString();
            }
            else
                Category = (AuthNodeCategory)reader.ReadUInt32();

            NodeSize = reader.ReadInt32();

            if (inf.file != AuthFile.BEP)
                ChildCount = reader.ReadInt32();
            else
                BEPDat.Unk = reader.ReadInt32();

            Flag = reader.ReadUInt32();

            if (inf.file == AuthFile.CMN)
            {
                Priority = reader.ReadInt32();
                reader.ReadBytes(12);
                Name = reader.ReadString(inf.version > 10 ? 32 : 16).Split(new[] { '\0' }, 2)[0];
            }
        }

        internal virtual void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            /*
            if (inf.file == AuthFile.CMN)
                unkBytes = reader.ReadBytes(NodeSize * 4);
            else if (inf.file == AuthFile.BEP)
            {
                BEPDat.DataUnk = reader.ReadInt32();
                unkBytes = reader.ReadBytes(NodeSize - 4);
            }
            */
        }

        internal void Write(DataWriter writer, GameVersion version, uint hactVer)
        {
            long preWrite = writer.Stream.Position;
            long sizeOffset = 0;

            if(!CMN.IsDE(version))
               sizeOffset = writer.Stream.Position + 20;
            else
               sizeOffset = writer.Stream.Position + (CMN.LastFile == AuthFile.BEP ? 68 : 20);

            long dataOffset = hactVer >= 18 ? writer.Stream.Position + 80 : writer.Stream.Position + 64;

            WriteNodeData(writer, version, hactVer);
            
            if (unkBytes != null && unkBytes.Length > 0)
                writer.Write(unkBytes);

            long writtenBytes = writer.Stream.Position - dataOffset;

            //Adjust size to amount of bytes written
            writer.Stream.RunInPosition(delegate 
            {
                if (CMN.LastFile == AuthFile.CMN)
                {
                    uint dividedSize = (uint)writtenBytes / 4;

                    if (hactVer > 10 && hactVer < 18)
                        dividedSize -= 4;

                    writer.Write(dividedSize);
                }
                else
                    writer.Write((int)writtenBytes);

            }, sizeOffset, SeekMode.Start);

            System.Diagnostics.Debug.Print($"Wrote {writtenBytes} bytes for node: " + Name + ", Category: " + Category);

            if (CMN.LastFile == AuthFile.CMN)
            {
                foreach (Node child in Children)
                    child.Write(writer, version, hactVer);
            }
        }

        protected void WriteCoreData(DataWriter writer, GameVersion version, uint hactVer)
        {
            writer.Write(Guid.ToByteArray());

            if (CMN.LastFile == AuthFile.BEP)
            {
                writer.Write(BEPDat.Guid2.ToByteArray());
                writer.WriteOfType(BEPDat.Bone);
                writer.Write((ushort)Category);

                if (Category == AuthNodeCategory.Element)
                    writer.Write((ushort)((this as NodeElement).ElementKind));
                else
                    writer.Write((ushort)0);
            }
            else
            {

                if (CMN.IsDE(version))
                    writer.Write((uint)Category);
                else
                {
                    if (Category == AuthNodeCategory.Element)
                        writer.Write((uint)AuthNodeCategory.Model_node);
                    else if (Category == AuthNodeCategory.Model_node)
                        writer.Write((uint)AuthNodeCategory.ModelMotion);
                    else if (Category == AuthNodeCategory.FolderCondition)
                        writer.Write((uint)AuthNodeCategory.InstanceMotionData);
                    else if (Category == AuthNodeCategory.Asset)
                        writer.Write((uint)AuthNodeCategory.CharacterBehavior);
                    else if (Category == AuthNodeCategory.ModelMotion)
                        writer.Write((uint)AuthNodeCategory.Asset);
                    else
                        writer.Write((uint)Category);
                }
            }

            //obsolete auto calculated now
            int size = unchecked((int)0xDEADC0DE);

            if (CMN.LastFile == AuthFile.CMN)
                size = size / 4;
            else if (CMN.LastFile == AuthFile.BEP)
                if (Category != AuthNodeCategory.Element)
                    size += 4;

            writer.Write(size);
            writer.Write(CMN.LastFile == AuthFile.BEP ? BEPDat.Unk : Children.Count);
            writer.Write(Flag);

            if (CMN.LastFile == AuthFile.CMN)
            {
                writer.Write(Priority);
                writer.WriteTimes(0, 12);

                int nameSize = hactVer > 10 ? 32 : 16;

                writer.Write(Name.ToLength(32), false, System.Text.Encoding.GetEncoding(932), nameSize);
            }
        }

        internal virtual void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            WriteCoreData(writer, version, hactVer);
        }

        internal virtual int GetSize(GameVersion version, uint hactVer)
        {
            if (unkBytes != null)
                return unkBytes.Length;
            else
                return unchecked((int)0xDEADC0DE);
        }

        
        public Node GetChildOfCategory(AuthNodeCategory category)
        {
            return Children.FirstOrDefault(x => x.Category == category);
        }

        public T GetChildOfType<T>() where T : Node
        {
            return (T)Children.FirstOrDefault(x => x is T);
        }

        public T[] GetChildsOfType<T>() where T : Node
        {
            return Children.Where(x => x is T).Cast<T>().ToArray();
        }

        /// <summary>
        /// Conversions that concern same engine different game
        /// </summary>
        public virtual bool TryConvert(Game input, Game output)
        {
            return true;
        }
    }
}
