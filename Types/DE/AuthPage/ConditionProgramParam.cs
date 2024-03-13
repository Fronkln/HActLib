using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionProgramParam : Condition
    {
        public uint FlagIndex;
        public uint Flag;

        internal override void Read(DataReader reader, uint parameterSize)
        {
            FlagIndex= reader.ReadUInt32();
            Flag = reader.ReadUInt32();
            reader.ReadBytes(8);
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(FlagIndex);
            writer.Write(Flag);
            writer.WriteTimes(0, 8);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
