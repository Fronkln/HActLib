using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class Set2ElementParticle : Set2Element
    {
        public uint ParticleID;
        public int Unknown1;
        public int Unknown2;
        public Matrix4x4 Matrix;

        public override string GetName()
        {
            return "Particle " + ParticleID;
        }

        /*
        internal override void ReadArgs(DataReader reader)
        {
            ParticleID = reader.ReadUInt32();

            Unk2 = reader.ReadBytes(248);
        }

        internal override void WriteArgs(DataWriter writer)
        {
            writer.Write(ParticleID);

            base.WriteArgs(writer);
        }
        */

        public override int GetElementID()
        {
            return 1002;
        }
    }
}
