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

        internal override void Read(DataReader reader, uint parameterSize)
        {
            Count = reader.ReadInt32();
            reader.ReadBytes(12);
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(Count);
            writer.WriteTimes(0, 12);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
