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
        public ConditionPageEnd()
        {
            ConditionID = (uint)ConditionType.page_end;
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
