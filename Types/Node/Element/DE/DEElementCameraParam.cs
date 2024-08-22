using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{

    [ElementID(Game.Y6, 0x1)]
    [ElementID(Game.YK2, 0x1)]
    [ElementID(Game.JE, 0x1)]
    [ElementID(Game.YLAD, 0x1)]
    [ElementID(Game.LJ, 0x1)]
    [ElementID(Game.LAD7Gaiden, 0x1)]
    [ElementID(Game.LADIW, 0x1)]
    public class DEElementCameraParam : NodeElement
    {
        public uint ParamFlags;
        
        public Vector3 BeforePos;
        public Vector3 BeforeIntr;
        public float BeforeFovY;
        public Vector3 BeforeUp;

        public Vector3 AfterPos;
        public Vector3 AfterIntr;
        public float AfterFovY;
        public Vector3 AfterUp;

        public uint RotateType;
        public float RotateX;
        public float RotateY;

        public uint AssetCaptureCameraType;
        public int AssetCaptureCameraTilt;

        public float[] Animation = new float[256];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ParamFlags = reader.ReadUInt32();

            BeforePos = reader.ReadVector3();
            BeforeIntr = reader.ReadVector3();
            BeforeFovY = reader.ReadSingle();
            BeforeUp = reader.ReadVector3();

            AfterPos = reader.ReadVector3();
            AfterIntr = reader.ReadVector3();
            AfterFovY = reader.ReadSingle();
            AfterUp = reader.ReadVector3();

            RotateType = reader.ReadUInt32();
            RotateX = reader.ReadSingle();
            RotateY = reader.ReadSingle();

            AssetCaptureCameraType = reader.ReadUInt32();
            AssetCaptureCameraTilt = reader.ReadInt32();

            Animation = new float[256];

            for (int i = 0; i < Animation.Length; i++)
                Animation[i] = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(ParamFlags);

            writer.Write(BeforePos);
            writer.Write(BeforeIntr);
            writer.Write(BeforeFovY);
            writer.Write(BeforeUp);

            writer.Write(AfterPos);
            writer.Write(AfterIntr);
            writer.Write(AfterFovY);
            writer.Write(AfterUp);

            writer.Write(RotateType);
            writer.Write(RotateX);
            writer.Write(RotateY);

            writer.Write(AssetCaptureCameraType);
            writer.Write(AssetCaptureCameraTilt);

            foreach (float f in Animation)
                writer.Write(f);
        }
    }
}
