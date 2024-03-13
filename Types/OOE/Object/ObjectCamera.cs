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
        public string Name = "Camera";

        public ObjectCamera() : base()
        {
            Type = ObjectNodeCategory.Camera;
            Category = 2147483647;

            UnkFloatDats[1] = new float[]
            {
                0.051175f,
                1.0129863f,
                0.30221808f,
                1.27064776f,
                1.04361045f,
                1.02616715f
            };
        }

        public override string GetName()
        {
            return Name;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);
            Name = StringTable[0];
        }

        internal override void WriteSetData(DataWriter writer)
        {
            StringTable[0] = Name;
            StringTable[1] = "";
            StringTable[2] = "";

            base.WriteSetData(writer);
        }

        internal override void ReadObjectData(DataReader reader)
        {
            reader.ReadBytes(292);
        }

        internal override void WriteObjectData(DataWriter writer)
        {
            writer.WriteTimes(0, 292);
        }
    }
}
