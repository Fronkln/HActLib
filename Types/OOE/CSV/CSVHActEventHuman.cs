using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CSVHActEventHuman : CSVHActEvent
    {
        public int Unknown;
        public byte[] Nothing;
        public CSVHActEventHuman()
        {
            Type = CSVHActEventType.Human;
            Nothing = new byte[24];
            Name = "HE_HUMAN_00";
        }

        internal override void ReadData(DataReader reader)
        {
            Unknown = reader.ReadInt32();
            Nothing = reader.ReadBytes(24);
        }

        internal override void WriteData(DataWriter writer)
        {
            base.WriteData(writer);
            writer.Write(Unknown);
            writer.Write(Nothing);
        }
    }
}
