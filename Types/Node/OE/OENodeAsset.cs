using Yarhl.IO;

namespace HActLib
{
    public class OENodeAsset : Node
    {
        public int Unknown1 = 0;
        public int Unknown2 = 1;
        public int Unknown3 = 0;
        public int Unknown4 = 0;

        public OENodeAsset()
        {
            Category = AuthNodeCategory.Asset;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 16;
        }
    }
}
