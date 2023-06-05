using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 2)]
    [ElementID(Game.Ishin, 2)]
    [ElementID(Game.Y0, 2)]
    public class OEParticle : NodeElement
    {
        public uint ParticleID = 0;
        public int Unknown;
        public int Unknown2 = 0;
        public int Unknown3 = 0;

        public Matrix4x4 Matrix = Matrix4x4.Default;
        public Vector3 Scale = new Vector3(1, 1, 1);

        public int Unknown4 = 0;
        public int Unknown5 = 0;

        public RGBA Color = new RGBA(255, 255, 255, 255);

        public int Unknown6 = 0;
        public float Unknown7 = 1;

        //Y0 and Above
        public Vector4 Unknown8 = new Vector4(1, -1, -1, 0);
        public byte[] Animation = new byte[32];

        public OEParticle() : base()
        {
            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = 255;
        }


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ParticleID = reader.ReadUInt32();
            Unknown = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();

            Matrix = Matrix4x4.Read(reader);
            Scale = reader.ReadVector3();

            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();

            Color = RGBA.Read(reader);
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadSingle(); //1 in Y0, 0 in Y5
            //---END OF Y5 DATA--

            if (inf.version > 15)
            {
                Unknown8 = reader.ReadVector4();
                Animation = reader.ReadBytes(32);
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(ParticleID);
            writer.Write(Unknown);
            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(Matrix);
            writer.Write(Scale);

            writer.Write(Unknown4);
            writer.Write(Unknown5);

            writer.Write(Color);
            writer.Write(Unknown6);
            writer.Write(Unknown7);

            if (CMN.LastHActDEGame > Game.Ishin)
            {
                writer.Write(Unknown8);
                writer.Write(Animation);
            }
        }

        public override void OE_ConvertToY0()
        {
            base.OE_ConvertToY5();

            Unknown7 = 1;
            Unknown8 = new Vector4(1, -1, -1, 0);
            Animation = new byte[32];

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = 255; //1.0f
        }

        public override void OE_ConvertToY5()
        {
            base.OE_ConvertToY5();

            Unknown7 = 0;
        }
    }
}
