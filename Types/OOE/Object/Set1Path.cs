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
        public string Description = "Path";
        public string UnkStr1 = "";
        public string UnkStr2 = "";

        public ObjectPath() : base()
        {
            Type = ObjectNodeCategory.Path;
            Category = 2147483647;
        }

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

        internal override void WriteSetData(DataWriter writer)
        {
            StringTable[0] = Description;
            StringTable[1] = UnkStr1;
            StringTable[2] = UnkStr2;

            base.WriteSetData(writer);
        }

        internal override void ReadObjectData(DataReader reader)
        {
            base.ReadObjectData(reader);

            reader.ReadBytes(292);
        }

        internal override void WriteObjectData(DataWriter writer)
        {
            base.WriteObjectData(writer);

            writer.WriteTimes(0, 292);
        }
    }
}
