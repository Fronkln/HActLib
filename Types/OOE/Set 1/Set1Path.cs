using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectPath : ObjectBase
    {
        public string Description;
        public string UnkStr1;
        public string UnkStr2;

        public override string GetName()
        {
            return Description;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);

            Description = StringTable[0];
            UnkStr1 = StringTable[1];
            UnkStr2 = StringTable[2];
        }
    }
}
