using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x8B)]
    [ElementID(Game.YK2, 0x8B)]
    [ElementID(Game.JE, 0x8B)]
    [ElementID(Game.YLAD, 0x87)]
    [ElementID(Game.LJ, 0x87)]
    [ElementID(Game.Gaiden, 0x87)]
    [ElementID(Game.Y8, 0x87)]
    public class DEElementCharacterChange : NodeElement
    {
        public uint CharacterID = 0x1;
        public uint Flags = 0x1;
        public byte Mask = 0x80;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            CharacterID = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            Mask = reader.ReadByte();
            reader.ReadBytes(7);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(CharacterID);
            writer.Write(Flags);
            writer.Write(Mask);
            writer.WriteTimes(0, 7);
        }
    }
}
