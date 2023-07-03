using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x66)]
    [ElementID(Game.YK2, 0x66)]
    [ElementID(Game.JE, 0x66)]
    [ElementID(Game.YLAD, 0x63)]
    [ElementID(Game.LJ, 0x63)]
    [ElementID(Game.LAD7Gaiden, 0x63)]
    [ElementID(Game.LADIW, 0x63)]
    public class DEElementCameraLink : NodeElement
    {
        public uint LinkFlags;
        public float RotationYOffsetDegrees;
        
        public Vector4 TrsPos;
        public Vector4 TrsIntr;
        public Vector4 TrsUp;
        
        public float TrsFovYRadians;
        public float TrsClipNear;
        public float TrsClipFar;

        public byte[] Animation = new byte[64];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            LinkFlags = reader.ReadUInt32();
            RotationYOffsetDegrees = reader.ReadSingle();

            reader.ReadBytes(8);

            TrsPos = reader.ReadVector4();
            TrsIntr = reader.ReadVector4();
            TrsUp = reader.ReadVector4();

            TrsFovYRadians = reader.ReadSingle();
            TrsClipNear = reader.ReadSingle();
            TrsClipFar = reader.ReadSingle();

            reader.ReadBytes(4);

            Animation = reader.ReadBytes(64);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(LinkFlags);
            writer.Write(RotationYOffsetDegrees);

            writer.WriteTimes(0, 8);

            writer.Write(TrsPos);
            writer.Write(TrsIntr);
            writer.Write(TrsUp);

            writer.Write(TrsFovYRadians);
            writer.Write(TrsClipNear);
            writer.Write(TrsClipFar);

            writer.WriteTimes(0, 4);

            writer.Write(Animation);
        }
    }
}
