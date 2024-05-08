using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionPlayCount : Condition
    {
        public uint PlayCount;
        public int Unk2;

        internal override void Read(DataReader reader, uint parameterSize)
        {
            PlayCount = reader.ReadUInt32();
            Unk2 = reader.ReadInt32();
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(PlayCount);
            writer.Write(Unk2);
        }

        internal override int Size()
        {
            return 8;
        }
    }
}
