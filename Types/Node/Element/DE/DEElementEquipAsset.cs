using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x74)]
    [ElementID(Game.YK2, 0x74)]
    [ElementID(Game.JE, 0x74)]
    [ElementID(Game.YLAD, 0x71)]
    [ElementID(Game.LJ, 0x71)]
    [ElementID(Game.Gaiden, 0x71)]
    [ElementID(Game.Y8, 0x71)]
    public class DEElementEquipAsset : NodeElement
    {
        public uint AssetID;
        public uint EquipFlags;
        public AttachmentSlot EquipSlot;
        public ulong UID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            AssetID = reader.ReadUInt32();
            EquipFlags = reader.ReadUInt32();
            EquipSlot = (AttachmentSlot)reader.ReadUInt32();
            reader.ReadBytes(4);
            UID = reader.ReadUInt64();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(AssetID);
            writer.Write(EquipFlags);
            writer.Write((uint)EquipSlot);
            writer.WriteTimes(0, 4);
            writer.Write(UID);
        }
    }
}
