using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    [ElementID(Game.Y6, 0xA)]
    [ElementID(Game.YK2, 0xA)]
    [ElementID(Game.JE, 0xA)]
    [ElementID(Game.YLAD, 0x9)]
    [ElementID(Game.LJ, 0x9)]
    [ElementID(Game.LAD7Gaiden, 0x9)]
    [ElementID(Game.LADIW, 0x9)]
    public class DEElementGradation : NodeElement
    {
        public bool IsAnimation;

        public float Depth;
        public float Deep;
        public float Power;

        public bool IgnoreBezier;

        public uint InFrame;
        public uint OutFrame;

        public RGBA32 ColorL1 = new RGBA32();

        public float PosL1;
        public float PosL2;

        public float Rotation;

        public RGBA32 ColorL1After = new RGBA32();

        public float PosL1After;
        public float PosL2After;

        public float RotateAfter;

        public byte Blend;

        public int IsCircular;
        public int IsTrans;
        public int IsGizmo;
        public int IsFollowMode;
        public int IsMultiple;

        private byte[] circleData;
        public byte[] Animation; //32

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            IsAnimation = reader.ReadInt32() == 1;

            Depth = reader.ReadSingle();
            Deep = reader.ReadSingle();
            Power = reader.ReadSingle();

            IgnoreBezier = reader.ReadInt32() == 1;

            InFrame = reader.ReadUInt32();
            OutFrame = reader.ReadUInt32();

            ColorL1 = (RGBA32)ConvertFormat.With<RGBA32Convert>(new ConversionInf(new BinaryFormat(reader.Stream), EndiannessMode.LittleEndian));

            PosL1 = reader.ReadSingle();
            PosL2 = reader.ReadSingle();

            Rotation = reader.ReadSingle();

            ColorL1After = (RGBA32)ConvertFormat.With<RGBA32Convert>(new ConversionInf(new BinaryFormat(reader.Stream), EndiannessMode.LittleEndian));

            PosL1After = reader.ReadSingle();
            PosL2After = reader.ReadSingle();

            RotateAfter = reader.ReadSingle();

            Blend = reader.ReadByte();
            reader.ReadBytes(3);

            IsCircular = reader.ReadInt32();
            IsTrans = reader.ReadInt32();
            IsGizmo = reader.ReadInt32();
            IsFollowMode = reader.ReadInt32();
            IsMultiple = reader.ReadInt32();

            circleData = reader.ReadBytes(0x64 * 3);
            Animation = reader.ReadBytes(32);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Convert.ToInt32(IsAnimation));

            writer.Write(Depth);
            writer.Write(Deep);
            writer.Write(Power);

            writer.Write(Convert.ToInt32(IgnoreBezier));

            writer.Write(InFrame);
            writer.Write(OutFrame);

            writer.Write(ColorL1.R);
            writer.Write(ColorL1.G);
            writer.Write(ColorL1.B);
            writer.Write(ColorL1.A);

            writer.Write(PosL1);
            writer.Write(PosL2);

            writer.Write(Rotation);

            writer.Write(ColorL1After.R);
            writer.Write(ColorL1After.G);
            writer.Write(ColorL1After.B);
            writer.Write(ColorL1After.A);

            writer.Write(PosL1After);
            writer.Write(PosL2After);

            writer.Write(RotateAfter);

            writer.Write(Blend);
            writer.WriteTimes(0, 3);

            writer.Write(IsCircular);
            writer.Write(IsTrans);
            writer.Write(IsGizmo);
            writer.Write(IsFollowMode);
            writer.Write(IsMultiple);

            writer.Write(circleData);
            writer.Write(Animation);
        }
    }
}
