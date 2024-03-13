using System;

using Yarhl.IO;

namespace HActLib
{
    public class DENodeCharacter : Node
    {
        public uint ScaleID;
        public uint OptionFlag;
        public ulong UID;
        public uint CharacterID;
        public HActReplaceID ReplaceID;
        public uint TagID;
        public int ForceAdv;
        public uint[] ChangeCharacterID = new uint[8];

        public DENodeCharacter()
        {
            Category = AuthNodeCategory.Character;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            ScaleID = reader.ReadUInt32();
            OptionFlag = reader.ReadUInt32();
            UID = reader.ReadUInt64();
            CharacterID = reader.ReadUInt32();
            ReplaceID = (HActReplaceID)reader.ReadUInt32();
            TagID = reader.ReadUInt32();
            ForceAdv = reader.ReadInt32();

            if (CMN.VersionGreater(version, GameVersion.Yakuza6Demo))
                for (int i = 0; i < ChangeCharacterID.Length; i++)
                    ChangeCharacterID[i] = reader.ReadUInt32();
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(ScaleID);
            writer.Write(OptionFlag);
            writer.Write(UID);
            writer.Write(CharacterID);
            writer.Write((uint)ReplaceID);
            writer.Write(TagID);
            writer.Write(ForceAdv);

            if (CMN.VersionGreater(version, GameVersion.Yakuza6Demo))
                foreach (uint f in ChangeCharacterID)
                    writer.Write(f);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 64 / 4;
        }
    }
}
