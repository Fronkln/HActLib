using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class Set2 : ITEVObject
    {
        public struct _Set2Internal
        {
            public uint Addr;

            public int resourcePtr;
            public int elementType; //only set if type == 9, else its 0
        }

        public Set2NodeCategory Type = 0;
        public string Resource = "";

        public float Start = 0;
        public float End = 0;
        public float Fps = 30;

        public int Unk1;
        public int Unk3;

        public EffectID EffectID;

        public float[] UnkSpeed = new float[8];
        public byte[] Unk2 = new byte[252];

        public byte[] Unk4 = new byte[16];

        public _Set2Internal _InternalInfo = new _Set2Internal();

        public virtual string GetName() => $"Unknown Set {Type}";

        internal static Set2 ReadFromMemory(DataReader reader)
        {
            uint type = 0;
            uint elementID = 0; //applicable if type == 9
            reader.Stream.RunInPosition
                (
                    delegate
                    {
                        type = reader.ReadUInt32();
                        reader.ReadBytes(4);
                        elementID = reader.ReadUInt32();
                    }, 8, SeekMode.Current
                );


            Set2 set = GetSet2Type(type, elementID);
            set._InternalInfo.Addr = (uint)reader.Stream.Position;

            set.ReadBasicSet2Info(reader);

            return set;
        }

        private void ReadBasicSet2Info(DataReader reader)
        {
            long startPos = reader.Stream.Position;
            long expectedPos = startPos + 336;

            uint index = reader.ReadUInt32();

            _InternalInfo.resourcePtr = reader.ReadInt32();

            Type = (Set2NodeCategory)reader.ReadInt32();
            Unk1 = reader.ReadInt32();
            _InternalInfo.elementType = reader.ReadInt32();
            EffectID = (EffectID)_InternalInfo.elementType;

            ReadArgs(reader);
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Fps = reader.ReadSingle();
            Unk3 = reader.ReadInt32();

            for (int i = 0; i < 8; i++)
                UnkSpeed[i] = reader.ReadSingle();

            long curReadPos = reader.Stream.Position;

            ProcessNodeData(reader);
            reader.Stream.Seek(curReadPos);

            Unk4 = reader.ReadBytes(16);

            if (reader.Stream.Position != expectedPos)
                throw new Exception($"TEV Read Error: Set2 size mismatch. Expected to read 336 bytes, got: {reader.Stream.Position - startPos}");
        }

        private static Set2 GetSet2Type(uint type, uint elementType)
        {
            switch (type)
            {
                default:
                    return new Set2();

                case 1:
                    return new Set2ElementMotion();
                case 2:
                    return new Set2ElementMotion();
                case 3:
                    return new Set2ElementMotion();
                case 9:
                    switch (elementType)
                    {
                        default:
                            return new Set2Element();
                        case 1001:
                            return new Set2ElementBaseScreenEffect();
                        case 1005:
                            return new Set2ElementBaseScreenEffect();
                        case 1006:
                            return new Set2ElementBaseScreenEffect();
                        case 1008:
                            return new Set2ElementBaseScreenEffect();
                        case 1009:
                            return new Set2ElementBaseScreenEffect();
                        case 1011:
                            return new Set2ElementBaseScreenEffect();
                        case 1019:
                            return new Set2Element1019();
                        case 1020:
                            return new Set2ElementBaseScreenEffect();
                        case 1023:
                            return new Set2ElementBaseScreenEffect();
                        case 1024:
                            return new Set2ElementBaseScreenEffect();
                    }
            }
        }

        internal virtual void ProcessNodeData(DataReader reader)
        {
            if (_InternalInfo.resourcePtr > -1)
            {
                reader.Stream.Seek(_InternalInfo.resourcePtr);
                Resource = reader.ReadString();
            }

            EffectID = (EffectID)_InternalInfo.elementType;
        }

        internal virtual void ReadArgs(DataReader reader)
        {
            Unk2 = reader.ReadBytes(252);
        }

        internal virtual void WriteSetData(DataWriter writer, bool alt)
        {
            writer.Write(_InternalInfo.resourcePtr);
            writer.Write((uint)Type);
            writer.Write(Unk1);
            writer.Write(GetElementID());

            WriteArgs(writer, alt);

            writer.Write(Start);
            writer.Write(End);
            writer.Write(Fps);
            writer.Write(Unk3);

            foreach(float f in UnkSpeed)
                writer.Write(f);

            writer.Write(Unk4);
        }

        internal virtual void WriteArgs(DataWriter writer, bool alt)
        {
            writer.Write(Unk2);
        }

        internal virtual int WriteResource(DataWriter writer)
        {
            if(!string.IsNullOrEmpty(Resource))
            {
                int pos = (int)writer.Stream.Position;
                writer.Write(Resource);

                return pos;
            }

            return -1;
        }

        public virtual int GetElementID()
        {
            return (int)EffectID;
        }
    }
}
