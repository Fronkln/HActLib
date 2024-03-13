using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 22)]
    [ElementID(Game.Ishin, 22)]
    public class OEFaceExpression : NodeElement
    {
        public int Unk1;
        public uint ExpressionID;
        public float Unk2;
        public float Unk3;
        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk1 = reader.ReadInt32();
            ExpressionID = reader.ReadUInt32();
            Unk2 = reader.ReadSingle();
            Unk3 = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unk1);
            writer.Write(ExpressionID);
            writer.Write(Unk2);
            writer.Write(Unk3);
        }
    }
}
