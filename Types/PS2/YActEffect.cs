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

        public byte[] UnknownData;

        internal virtual void ReadData(DataReader reader)
        {
            Unknown2 = reader.ReadInt32();
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Unknown3 = reader.ReadSingle();
            UnknownData = reader.ReadBytes(80);
        }

        internal virtual void WriteData(DataWriter writer)
        {
            writer.Write(Unknown2);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(Unknown3);
            writer.Write(UnknownData);
        }
    }
}
