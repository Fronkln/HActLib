using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x12A)]
    [ElementID(Game.YK2, 0x12A)]
    [ElementID(Game.JE, 0x12A)]
    [ElementID(Game.YLAD, 0x124)]
    [ElementID(Game.LJ, 0x124)]
    [ElementID(Game.LAD7Gaiden, 0x124)]
    [ElementID(Game.LADIW, 0x124)]
    [ElementID(Game.LADPYIH, 0x124)]
    [ElementID(Game.YK3, 0x124)]
    public class DEElementCharacterSpeed : NodeElement
    {
        public float MaxSpeed;
        public float MinSpeed;

        public byte[] Animation = new byte[32];

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            MaxSpeed = reader.ReadSingle();
            MinSpeed = reader.ReadSingle();

            reader.ReadBytes(8);

            Animation = reader.ReadBytes(Animation.Length);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(MaxSpeed);
            writer.Write(MinSpeed);

            writer.WriteTimes(0, 8);
            writer.Write(Animation);
        }
    }
}
