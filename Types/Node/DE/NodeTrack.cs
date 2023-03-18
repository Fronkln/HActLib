using System;
using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    public class DENodePath : NodePathBase
    {
        public uint PathFlags;

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadNodeData(reader, inf, version);
           
            PathFlags = reader.ReadUInt32();
            reader.ReadBytes(12);
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(PathFlags);
            writer.WriteTimes(0, 12);

        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 80 / 4;
        }
    }
}
