using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y0, 22)]
    [ElementID(Game.YK1, 22)]
    [ElementID(Game.FOTNS, 23)]
    public class OEExpressionTarget : NodeElement
    {
        public class ExpressionData
        {
            public byte[] AnimationData;
        }

        public enum Type
        {
            Unknown1,
            Unknown2,
            Unknown3,
            Unknown4,
            Unknown5,
            pain,
            Unknown7,
            Unknown8,
            Unknown9,
            Unknown10,
            Unknown11,
            Unknown12,
            Unknown13,
        }

        public ExpressionData[] Datas;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            int numDatas = (inf.expectedSize - 32) / 32;

            Datas = new ExpressionData[numDatas];

            for(int i = 0; i < numDatas; i++)
            {
                ExpressionData dat = new ExpressionData();
                dat.AnimationData = reader.ReadBytes(32);

                Datas[i] = dat;
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            foreach (ExpressionData dat in Datas)
                writer.Write(dat.AnimationData);
        }
    }
}
