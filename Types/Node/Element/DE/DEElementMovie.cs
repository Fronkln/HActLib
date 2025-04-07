using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x9B)]
    [ElementID(Game.YK2, 0x9B)]
    [ElementID(Game.JE, 0x9B)]
    [ElementID(Game.YLAD, 0x97)]
    [ElementID(Game.LJ, 0x97)]
    [ElementID(Game.LAD7Gaiden, 0x97)]
    [ElementID(Game.LADIW, 0x97)]
    [ElementID(Game.LADPYIH, 0x97)]
    public class DEElementMovie : NodeElement
    {
        public int MovieVersion = 0;
        public float FadeFrame = 0;
        public bool DontFade = true;
        public string Movie = "";

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            MovieVersion = reader.ReadInt32();
            if (version < GameVersion.DE3)
                FadeFrame = reader.ReadSingle();
            else
                FadeFrame = new GameTick2(reader.ReadUInt32()).Frame;
            DontFade = reader.ReadInt32() == 1;
            Movie = reader.ReadString();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(MovieVersion);
            if (version < GameVersion.DE3)
                writer.Write(FadeFrame);
            else
                writer.Write(new GameTick2(FadeFrame).Tick);

            writer.Write(Convert.ToInt32(DontFade));
            writer.Write(Movie);
        }
    }
}
