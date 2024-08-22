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
        public uint ParticleID = 0;
        public uint Flag;
        public int Unknown2;
        public Matrix4x4 Matrix = Matrix4x4.Default;
        public Vector3 Unknown3 = new Vector3(1, 1, 1);
        public int Unknown4;
        public Vector3 Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        //99% of it are zeroes
        public float[] UnknownFloats = new float[132 / 4];

        public EffectParticle() : base()
        {
            ElementKind = EffectID.Particle;
            EffectID2 = EffectID.Particle;
        }

        internal override void ReadEffectData(DataReader reader, bool alt)
        {
            base.ReadEffectData(reader, alt);

            ParticleID = reader.ReadUInt32();
            Flag = reader.ReadUInt32();
            Unknown2 = reader.ReadInt32();
            Matrix = reader.ReadMatrix4x4();

            reader.Stream.Position += 4;

            Unknown3 = reader.ReadVector3();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadVector3();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();

            reader.Stream.Position += 132;
        }

        internal override void WriteEffectData(DataWriter writer, bool alt)
        {
            base.WriteEffectData(writer, alt);

            writer.Write(ParticleID);
            writer.Write(Flag);
            writer.Write(Unknown2);
            writer.Write(Matrix);

            writer.WriteTimes(0, 4);

            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);

            writer.WriteTimes(0, 132);
        }
    }
}
