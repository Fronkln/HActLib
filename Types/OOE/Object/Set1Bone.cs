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
        public string BoneName = "center_n";
        public int BoneID = 0;

        public ObjectBone() : base()
        {
            Type = ObjectNodeCategory.Bone;
            Category = 2147483647;

            UnkFloatDats[0] = null;
        }

        public override string GetName()
        {
            return (string.IsNullOrEmpty(BoneName) ? "Bone" : BoneName);
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);
            BoneName = StringTable[0];
        }

        internal override void WriteSetData(DataWriter writer)
        {
            StringTable[0] = BoneName;
            StringTable[0] = StringTable[0].ToLength(30); //loll

            base.WriteSetData(writer);
        }

        internal override void ReadObjectData(DataReader reader)
        {
            reader.ReadBytes(256);

            BoneID = reader.ReadInt32();
           
            reader.ReadBytes(32);
        }

        internal override void WriteObjectData(DataWriter writer)
        {
            writer.WriteTimes(0, 256);

            writer.Write(BoneID);

            writer.WriteTimes(0, 32);
        }
    }
}
