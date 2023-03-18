using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectItem : ObjectBase
    {
        public string Description;

        public override string GetName()
        {
            return base.GetName();
        }
        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);

            Description = StringTable[0];
        }
    }
}
