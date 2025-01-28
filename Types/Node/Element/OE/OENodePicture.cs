using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 11)]
    [ElementID(Game.Ishin, 11)]
    [ElementID(Game.Y0, 11)]
    [ElementID(Game.FOTNS, 12)]
    public class OENodePicture : NodeElement
    {
        public int Unknown;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public float BeforeCenterX;
        public float BeforeCenterY;
        public float BeforeSizeX;
        public float BeforeSizeY;
        public float Unknown8;
        public float Unknown9;
        public float AfterCenterX;
        public float AfterCenterY;
        public float AfterSizeX;
        public float AfterSizeY;
        public float Unknown10;
        public float Unknown11;

        private byte[] Unk;

        public byte[] Animation;
        public string PictureName;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            
            BeforeCenterX = reader.ReadSingle();
            BeforeCenterY = reader.ReadSingle();
            BeforeSizeX = reader.ReadSingle();
            BeforeSizeY = reader.ReadSingle();

            reader.ReadBytes(8);

            Unknown8 = reader.ReadSingle();
            Unknown9 = reader.ReadSingle();

            AfterCenterX = reader.ReadSingle();
            AfterCenterY = reader.ReadSingle();
            AfterSizeX = reader.ReadSingle();
            AfterSizeY = reader.ReadSingle();

            reader.ReadBytes(8);

            Unknown10 = reader.ReadSingle();
            Unknown11 = reader.ReadSingle();

            Unk = reader.ReadBytes(12);
            Animation = reader.ReadBytes(32);
            PictureName = reader.ReadString(32).Split(new[] { '\0' }, 2)[0]; ;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Unknown);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);

            writer.Write(BeforeCenterX);
            writer.Write(BeforeCenterY);
            writer.Write(BeforeSizeX);
            writer.Write(BeforeSizeY);

            writer.WriteTimes(0, 8);

            writer.Write(Unknown8);
            writer.Write(Unknown9);

            writer.Write(AfterCenterX);
            writer.Write(AfterCenterY);
            writer.Write(AfterSizeX);
            writer.Write(AfterSizeY);

            writer.WriteTimes(0, 8);

            writer.Write(Unknown10);
            writer.Write(Unknown11);

            writer.Write(Unk);
            writer.Write(Animation);
            writer.Write(PictureName.ToLength(32), false, maxSize: 32);
        }
    }
}
