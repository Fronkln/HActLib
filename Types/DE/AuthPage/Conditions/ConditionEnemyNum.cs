using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionEnemyNum : Condition
    {
        public int Count;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;

        internal override void Read(DataReader reader, uint parameterSize)
        {
            Count = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(Count);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
