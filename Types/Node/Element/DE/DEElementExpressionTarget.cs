using System;
using System.Collections.Generic;
using System.Linq;
using Yarhl.IO;

namespace HActLib
{
    public class ExpressionTargetData
    {
        public DEFaceTarget FaceTargetID;
        public byte[] Animation = new byte[256];
    }

    [ElementID(Game.Y6, 0xB3)]
    [ElementID(Game.YK2, 0xB3)]
    [ElementID(Game.JE, 0xB3)]
    [ElementID(Game.YLAD, 0xAF)]
    [ElementID(Game.LJ, 0xAF)]
    [ElementID(Game.LAD7Gaiden, 0xAF)]
    [ElementID(Game.LADIW, 0xAF)]
    [ElementID(Game.LADPYIH, 0xAF)]
    [ElementID(Game.YK3, 0xAF)]
    public class DEElementExpressionTarget : NodeElement
    {
        public List<ExpressionTargetData> Data = new List<ExpressionTargetData>();

        private int Unk1;
        private int Unk2;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            int dataCount = reader.ReadInt32();

            if (version > GameVersion.Yakuza6)
                Unk1 = reader.ReadInt32();

            if (Version > 0)
                Unk2 = reader.ReadInt32();

            for (int i = 0; i < dataCount; i++)
            {
                ExpressionTargetData dat = new ExpressionTargetData();

                dat.FaceTargetID = (DEFaceTarget)reader.ReadUInt32();
                dat.Animation = reader.ReadBytes(256);

                Data.Add(dat);
            }

        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Data.Count);

            if (version > GameVersion.Yakuza6)
                writer.Write(Unk1);

            if (version <= GameVersion.DE1)
                Version = 0;

            if (Version > 0)
                writer.Write(Unk2);

            // writer.Write(Data.Count);


            foreach (ExpressionTargetData dat in Data)
            {
                if (dat.Animation != null && dat.Animation.Length > 0 && dat.Animation.Length < 256)
                    dat.Animation = Utils.ConvertTo256PointCurve(dat.Animation);

                writer.Write((uint)dat.FaceTargetID);
                writer.Write(dat.Animation);
            }
        }
    }
}
