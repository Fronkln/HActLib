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
        public string Replace = "";
        public string ObjectType = "NORMAL_HUMAN_OBJECT";
        public string BodyType = "BODY";

        public string HumanUnk1 = "mc_crik_"; //What the fuck is this shit?

        public int Height = 185;
        public int HumanUnk3 = 41;

        public ObjectHuman() : base()
        {
            Type = ObjectNodeCategory.HumanOrWeapon;
            Category = 0;
        }

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
            StringTable[0] = Replace;
            StringTable[1] = ObjectType;
            StringTable[2] = BodyType;

            base.WriteSetData(writer);
        }

        internal override void ReadObjectData(DataReader reader)
        {
            base.ReadObjectData(reader);

            HumanUnk1 = reader.ReadString(32);

            //Assumed padding
            reader.ReadBytes(220);

            Height = reader.ReadInt32();
            HumanUnk3 = reader.ReadInt32();

            reader.ReadBytes(32);
        }

        internal override void WriteObjectData(DataWriter writer)
        {
            base.WriteObjectData(writer);

            writer.Write(HumanUnk1.ToLength(32));

            writer.WriteTimes(0, 220);

            writer.Write(Height);
            writer.Write(HumanUnk3);

            writer.WriteTimes(0, 32);
        }
    }
}
