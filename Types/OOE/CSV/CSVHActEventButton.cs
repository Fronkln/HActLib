using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CSVHActEventButton : CSVHActEvent
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;

        internal override void ReadData(DataReader reader)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
        }

        internal override void WriteData(DataWriter writer)
        {
            base.WriteData(writer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
        }
    }
}
