using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectBone : ObjectBase
    {
        public string BoneName;

        public override string GetName()
        {
            return (string.IsNullOrEmpty(BoneName) ? "Bone" : BoneName);
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);
            BoneName = StringTable[0];
        }
    }
}
