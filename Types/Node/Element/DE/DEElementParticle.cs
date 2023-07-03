using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    [ElementID(Game.LAD7Gaiden, 0x12)]
    [ElementID(Game.LADIW, 0x12)]
    public class DEElementParticle : NodeElement
    {
        public uint ParticleID = 1;
        public uint ParticleFlag = 15;
        public float NearFadeDistance = 0;
        public float NearFadeOffset = 0;

        private byte[] Unknown_LJ = new byte[12];
        public Matrix4x4 Matrix = Matrix4x4.Default;

        public Vector3 Scale = new Vector3(1, 1, 1);

        public float CameraFrontDistance;
        public float ForceY;

        public RGBA Color;
        public uint TickOffset;

        public float TickScale = 1;
        public float ColorScaleA = 1;
        public float InFrame = -1;
        public float OutFrame = -1;
        public float DisableFrameChangeMotion = 0;
        public byte[] Animation; //32

        public Vector4 VectorQuaternion = new Vector4(0, 0, 0, 1);

        public float VectorScale = 1;
        public float DofDepthBias = 32;
        public float DofAlphaMax = 1;
        public float DofAlphaMin = 0;

        public float EmissivePower;
        public float EmissivePowerOffset;

        public string ParticleName = "AAa0000"; //8 bytes

        public uint TickStart;

        public uint RenderTiming;
        public uint ParticleFlags2;

        public uint UISort;



        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            if (CMN.LastHActDEGame > Game.YLAD)
                reader.ReadUInt32(); //version repeating

            ParticleID = reader.ReadUInt32();
            ParticleFlag = reader.ReadUInt32();
            NearFadeDistance = reader.ReadSingle();
            NearFadeOffset = reader.ReadSingle();

            if (CMN.LastHActDEGame >= Game.LJ)
                Unknown_LJ = reader.ReadBytes(12);

            Matrix = Matrix4x4.Read(reader);
            Scale = Vector3.Read(reader);

            CameraFrontDistance = reader.ReadSingle();
            ForceY = reader.ReadSingle();

            Color = RGBA.Read(reader);

            /*
            TickOffset = reader.ReadUInt32();
            TickScale = reader.ReadSingle();
            ColorScaleA = reader.ReadSingle();
            InFrame = reader.ReadSingle();
            OutFrame = reader.ReadSingle();
            DisableFrameChangeMotion = reader.ReadSingle();
            Animation = reader.ReadBytes(32);

            
            VectorQuaternion = reader.ReadVector4();
            VectorScale = reader.ReadSingle();
            DofDepthBias = reader.ReadSingle();
            DofAlphaMax = reader.ReadSingle();

            if (version <= GameVersion.Yakuza6)
            {
                reader.ReadBytes(4);
                return;
            }

            if (CMN.LastHActDEGame < Game.LJ)
                DofAlphaMin = reader.ReadSingle();

            if (CMN.LastHActDEGame == Game.YLAD)
            {
                EmissivePower = reader.ReadSingle();
                EmissivePowerOffset = reader.ReadSingle();
            }

            ParticleName = reader.ReadString(8);
            TickStart = reader.ReadUInt32();
            RenderTiming = reader.ReadUInt32();


            if (CMN.LastHActDEGame >= Game.YLAD)
            {
                ParticleFlags2 = reader.ReadUInt32();
                UISort = reader.ReadUInt32();
            }

            reader.ReadBytes(16);

            if (CMN.LastHActDEGame >= Game.LJ)
                reader.ReadBytes(12);
            */
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

            if (CMN.LastHActDEGame >= Game.LJ)
                writer.WriteTimes(0, 12);

            Matrix.Write(writer);
            Scale.Write(writer);

            writer.Write(CameraFrontDistance);
            writer.Write(ForceY);

            Color.Write(writer);

            /*
            writer.Write(TickOffset);
            writer.Write(TickScale);
            writer.Write(ColorScaleA);
            writer.Write(InFrame);
            writer.Write(OutFrame);
            writer.Write(DisableFrameChangeMotion);
            writer.Write(Animation);
            
            writer.Write(VectorQuaternion);
            writer.Write(VectorScale);
            writer.Write(DofDepthBias);
            writer.Write(DofAlphaMax);

            if (version <= GameVersion.Yakuza6)
            {
                writer.WriteTimes(0, 4);
                return;
            }

            if (CMN.LastHActDEGame < Game.LJ)
                writer.Write(DofAlphaMin);

            if (CMN.LastHActDEGame == Game.YLAD)
            {
                writer.Write(EmissivePower);
                writer.Write(EmissivePowerOffset);
            }

            writer.Write(ParticleName.ToString());
            writer.Write(TickStart);
            writer.Write(RenderTiming);

            if (CMN.LastHActDEGame >= Game.YLAD)
            {
                writer.Write(ParticleFlags2);
                writer.Write(UISort);
            }

            writer.WriteTimes(0, 16);

            if (CMN.LastHActDEGame >= Game.LJ)
                writer.WriteTimes(0, 12);
            */
        }
    }
}
