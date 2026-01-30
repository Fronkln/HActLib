using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x98)]
    [ElementID(Game.YK2, 0x98)]
    [ElementID(Game.JE, 0x98)]
    [ElementID(Game.YLAD, 0x94)]
    [ElementID(Game.LJ, 0x94)]
    [ElementID(Game.LAD7Gaiden, 0x94)]
    [ElementID(Game.LADIW, 0x94)]
    [ElementID(Game.LADPYIH, 0x94)]
    [ElementID(Game.YK3, 0x94)]
    public class DEElementBattleShoot : NodeElement
    {
        public uint Param = 0;
        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Param = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Param);
        }
    }
}
