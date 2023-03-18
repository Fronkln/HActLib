using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x9D)]
    [ElementID(Game.YK2, 0x9D)]
    [ElementID(Game.JE, 0x9D)]
    [ElementID(Game.YLAD, 0x99)]
    [ElementID(Game.LJ, 0x99)]
    [ElementID(Game.Gaiden, 0x99)]
    [ElementID(Game.Y8, 0x99)]
    public class DEElementBattleHeat : NodeElement
    {
        public int HeatChange;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            HeatChange = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(HeatChange);
        }
    }
}
