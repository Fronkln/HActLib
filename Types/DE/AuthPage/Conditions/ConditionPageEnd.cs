using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class ConditionPageEnd : Condition
    {
        public ConditionPageEnd(Game game)
        {
            ConditionID = ConditionConvert.GetID("page_end", game);
        }

        internal override void Read(DataReader reader, uint parameterSize)
        {

        }

        internal override void Write(DataWriter writer)
        {

        }

        internal override int Size()
        {
            return 0;
        }
    }
}
