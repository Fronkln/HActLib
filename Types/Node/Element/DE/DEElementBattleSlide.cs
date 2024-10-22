using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 80)]
    [ElementID(Game.YK2, 80)]
    [ElementID(Game.JE, 80)]
    [ElementID(Game.YLAD, 77)]
    [ElementID(Game.LJ, 77)]
    [ElementID(Game.LAD7Gaiden, 77)]
    [ElementID(Game.LADIW, 77)]
    [ElementID(Game.LADPYIH, 77)]
    public class DEElementBattleSlide : NodeElement
    {
        public int SlideType = 0;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            SlideType = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(SlideType);
        }
    }
}
