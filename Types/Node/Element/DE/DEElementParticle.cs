using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x13)]
    [ElementID(Game.YK2, 0x13)]
    [ElementID(Game.JE, 0x13)]
    [ElementID(Game.YLAD, 0x12)]
    [ElementID(Game.LJ, 0x12)]
    [ElementID(Game.Gaiden, 0x12)]
    [ElementID(Game.Y8, 0x12)]
    public class DEElementParticle : NodeElement
    {
        public uint ParticleID;
        public uint ParticleFlag;
        public float NearFadeDistance;
        public float NearFadeOffset;

        Matrix4x4 Matrix;

        public Vector3 Scale;

        public float CameraFrontDistance;
        public float ForceY;

        public RGBA Color;
        public uint TickOffset;

        public float TickScale;
        public float ColorScaleA;
        public float InFrame;
        public float OutFrame;
        public float DisableFrameChangeMotion;
        public byte[] Animation; //32

        public Quaternion VectorQuaternion;

        public float VectorScale;
        public float DofDepthBias;
        public float DofAlphaMax;

        public float EmissivePower;
        public float EmissivePowerOffest;

        public string ParticleName; //8 bytes
        public uint TickStart;

        public uint RenderTiming;
        public uint ParticleFlags2;

        public uint UISort;

        //16 byte pad

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            if (CMN.LastHActDEGame > Game.YLAD)
                reader.ReadUInt32(); //version repeating

            ParticleID = reader.ReadUInt32();
            ParticleFlag = reader.ReadUInt32();
            NearFadeDistance = reader.ReadSingle();
            NearFadeOffset = reader.ReadSingle();

            Matrix = Matrix4x4.Read(reader);
            Scale = Vector3.Read(reader);

            CameraFrontDistance = reader.ReadSingle();
            ForceY = reader.ReadSingle();

            Color = RGBA.Read(reader);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            if (CMN.LastHActDEGame > Game.YLAD)
            {
                Version = 3;
                writer.Write(Version);
            }
            else
                Version = 0;

            writer.Write(ParticleID);
            writer.Write(ParticleFlag);
            writer.Write(NearFadeDistance);
            writer.Write(NearFadeOffset);

            Matrix.Write(writer);
            Scale.Write(writer);

            writer.Write(CameraFrontDistance);
            writer.Write(ForceY);

            Color.Write(writer);
        }
    }
}
