using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectCamera : ObjectBase
    {
        public string Name;
        public override string GetName()
        {
            return Name;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);
            Name = StringTable[0];
        }
    }
}
