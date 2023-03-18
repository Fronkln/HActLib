using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectHuman : ObjectBase
    {
        public string Replace;
        public string ObjectType;
        public string BodyType;
        public override string GetName()
        {
            return Replace;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);

            Replace = StringTable[0];
            ObjectType = StringTable[1];
            BodyType = StringTable[2];
        }

        internal override void WriteSetData(DataWriter writer)
        {
            base.WriteSetData(writer);
        }
    }
}
