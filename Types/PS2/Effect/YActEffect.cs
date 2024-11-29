using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActEffect
    {
        //These two only available in Y2
        public string Name = "EFFECT";
        public string Unknown1;

        public int Unknown2;

        public float Start;
        public float End;
        public float Unknown3;
        public int BoneID;

        public byte[] UnknownDataPre;
        public int Type;
        public byte[] UnknownDataPre2;
        public byte[] UnknownData;

        internal virtual void ReadData(DataReader reader)
        {
            long end = reader.Stream.Position + 96;

            Unknown2 = reader.ReadInt32();
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Unknown3 = reader.ReadSingle();
            BoneID = reader.ReadInt32();

            UnknownDataPre = reader.ReadBytes(24);
            Type = reader.ReadInt32();
            UnknownDataPre2 = reader.ReadBytes(12);
            ReadEffectData(reader);

            if (reader.Stream.Position < end)
                UnknownData = reader.ReadBytes((int)(end - reader.Stream.Position));
        }

        internal virtual void WriteData(DataWriter writer)
        {
            writer.Write(Unknown2);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(Unknown3);

            writer.Write(UnknownDataPre);
            writer.Write(Type);
            writer.Write(UnknownDataPre2);
            WriteEffectData(writer);
            writer.Write(UnknownData);
        }

        internal virtual void ReadEffectData(DataReader reader)
        {

        }

        internal virtual void WriteEffectData(DataWriter writer)
        {

        }
    }
}
