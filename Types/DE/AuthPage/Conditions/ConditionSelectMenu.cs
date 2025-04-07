using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionSelectMenu : Condition
    {
        public uint Choice;

        public ConditionSelectMenu(Game game)
        {
            ConditionID = ConditionConvert.GetID("select_menu", game);
        }

        internal override void Read(DataReader reader, uint parameterSize)
        {
            Choice = reader.ReadUInt32();
            reader.ReadBytes(12);
        }

        internal override void Write(DataWriter writer)
        {
            writer.Write(Choice);
            writer.WriteTimes(0, 12);
        }

        internal override int Size()
        {
            return 16;
        }
    }
}
