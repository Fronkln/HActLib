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
        public string Name = "EFFECT";
        public string Unknown1;

        public int Unknown2;

        public float Start;
        public float End;
        public float Unknown3;

        internal virtual void ReadData(DataReader reader)
        {
            Unknown2 = reader.ReadInt32();

            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Unknown3 = reader.ReadSingle();
        }

        internal virtual void WriteData(DataWriter writer)
        {

        }
    }
}
