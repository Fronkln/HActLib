using System;
using Yarhl.IO;
namespace HActLib
{

    [ElementID(Game.Y6, 0x55)]
    [ElementID(Game.YK2, 0x55)]
    [ElementID(Game.JE, 0x55)]
    [ElementID(Game.YLAD, 0x52)]
    [ElementID(Game.LJ, 0x52)]
    [ElementID(Game.LAD7Gaiden, 0x52)]
    [ElementID(Game.LADIW, 0x52)]
    [ElementID(Game.LADPYIH, 0x52)]
    [ElementID(Game.YK3, 0x52)]
    public class DEElementBattleThrow : NodeElement
    {
        public int Value = 0;
        public int Value2 = 0;
        public int Value3 = 0;
        public int Value4 = 0;
        public int Value5 = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Value = reader.ReadInt32();
            Value2 = reader.ReadInt32();

            if(CMN.LastHActDEGame == Game.LADIW)
            {
                Value3 = reader.ReadInt32();
                Value4 = reader.ReadInt32();
                Value5 = reader.ReadInt32();
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Value);
            writer.Write(Value2);

            if(CMN.LastHActDEGame == Game.LADIW)
            {
                writer.Write(Value3);
                writer.Write(Value4);
                writer.Write(Value5);
            }
        }
    }
}
