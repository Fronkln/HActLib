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
        public byte[] Unk2 = new byte[240];

        public byte[] Unk4 = new byte[16];

        public _Set2Internal _InternalInfo = new _Set2Internal();

        public virtual string GetName() => $"Unknown Set {Type}";

        internal virtual void ProcessNodeData(Yarhl.IO.DataReader reader)
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

        internal virtual void WriteSetData(DataWriter writer)
        {
            writer.Write(_InternalInfo.resourcePtr);
            writer.Write((uint)Type);
            writer.Write(Unk1);
            writer.Write(GetElementID());

            WriteArgs(writer);

            writer.Write(Start);
            writer.Write(End);
            writer.Write(Fps);
            writer.Write(Unk3);

            foreach(float f in UnkSpeed)
                writer.Write(f);

            writer.Write(Unk4);
        }

        internal virtual void WriteArgs(DataWriter writer)
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
