using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public enum ElementPlayType
    {
        Normal = 0x0,
        Oneshot = 0x1,
        Always = 0x2,
    };


    public class NodeElement : Node
    {
        public uint ElementKind;
        public float Start;
        public float End;
        public int Version;
        public uint ElementFlag;
        public ElementPlayType PlayType;
        public uint UpdateTimingMode;

        public NodeElement()
        {
            Category = AuthNodeCategory.Element;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            //Why does bep have this twice? brug
            ElementKind = reader.ReadUInt32();

            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Version = reader.ReadInt32();

            if (inf.version > 10)
            {
                ElementFlag = reader.ReadUInt32();
                PlayType = (ElementPlayType)reader.ReadUInt32();
                UpdateTimingMode = reader.ReadUInt32();

                if (inf.version < 18)
                    reader.ReadBytes(4);
            }

            if (inf.version >= 18)
                reader.ReadBytes(4);

            ReadElementData(reader, inf, version);

            //base.ReadNodeData(inf);
        }

        internal virtual void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            if(inf.file == AuthFile.MEP)
            {
                unkBytes = reader.ReadBytes(NodeSize);
                return;
            }

            if (inf.version > 10)
            {
                if (inf.version < 18)
                    unkBytes = reader.ReadBytes(NodeSize * 4 - 32);
                else
                {
                    if (inf.file == AuthFile.CMN)
                        unkBytes = reader.ReadBytes(NodeSize * 4 - 32);
                    else
                        unkBytes = reader.ReadBytes(NodeSize - 32);
                }
            }
            else
                unkBytes = reader.ReadBytes(NodeSize * 4 - 16);
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(ElementKind);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(Version);

            if (hactVer > 10)
            {
                writer.Write(ElementFlag);
                writer.Write((uint)PlayType);
                writer.Write(UpdateTimingMode);

                writer.WriteTimes(0, 4);
            }          

            //assumes there are no unk bytes
            WriteElementData(writer, version);
        }

        internal virtual void WriteElementData(DataWriter writer, GameVersion version)
        {
            // throw new NotImplementedException();
        }

        //change getsize to uint version later! this is important if you want to add y5 writing
        internal override int GetSize(GameVersion version, uint hactVer)
        {
            if (hactVer > 10)
            {
                if (hactVer < 18)
                    return (unkBytes.Length + 32) / 4;

                if (CMN.LastFile == AuthFile.CMN)
                    return (unkBytes.Length + 28) / 4;
                else if (CMN.LastFile == AuthFile.BEP)
                    return (unkBytes.Length + 28);

                return 0xDEAD;
            }
            else
                return (unkBytes.Length + 16) / 4;
        }


        public virtual void OE_ConvertToY0()
        {

        }

        public virtual void OE_ConvertToY5()
        {

        }
    }
}
