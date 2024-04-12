using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionHActFlag : Condition
    {
        public uint ConditionFlagOn;
        public uint ConditionFlagOff;

        public ConditionHActFlag(Game game)
        {
            ConditionID = ConditionConvert.GetID("hact_condition_flag", game);
        }

        public ConditionHActFlag(Game game, uint on, uint off)
        {
            ConditionID = ConditionConvert.GetID("hact_condition_flag", game);

            ConditionFlagOn = on;
            ConditionFlagOff = off;
        }

        internal override void Read(DataReader reader, uint parameterSize)
        {
            ConditionFlagOn = reader.ReadUInt32();
            ConditionFlagOff = reader.ReadUInt32();

            reader.ReadBytes(8);
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(ConditionFlagOn);
            writer.Write(ConditionFlagOff);
           
            writer.WriteTimes(0, 8);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
