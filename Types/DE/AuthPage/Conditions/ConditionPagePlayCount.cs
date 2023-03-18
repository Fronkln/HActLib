using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionPagePlayCount : Condition
    {
        public uint PlayCount;

        internal override void Read(DataReader reader, uint parameterSize)
        {
            PlayCount = reader.ReadUInt32();
            reader.ReadBytes(12);
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(PlayCount);
            writer.WriteTimes(0, 12);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
