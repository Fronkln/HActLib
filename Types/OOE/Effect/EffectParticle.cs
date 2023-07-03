using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class EffectParticle : EffectElement
    {
        public uint ParticleID;
        public int Unknown1;
        public int Unknown2;
        public Matrix4x4 Matrix;

        internal override void ReadEffectData(DataReader reader, bool alt)
        {
            base.ReadEffectData(reader, alt);

            ParticleID = reader.ReadUInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Matrix = reader.ReadMatrix4x4();
        }

        internal override void WriteEffectData(DataWriter writer, bool alt)
        {
            base.WriteEffectData(writer, alt);

            writer.Write(ParticleID);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Matrix);
        }
    }
}
