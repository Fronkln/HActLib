using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x10C)]
    [ElementID(Game.YK2, 0x10C)]
    [ElementID(Game.JE, 0x10C)]
    [ElementID(Game.YLAD, 0x107)]
    [ElementID(Game.LAD7Gaiden, 0x107)]
    [ElementID(Game.LADIW, 0x107)]
    public class DEElementAssetBreakUID : NodeElement
    {
        public uint Ver;
        public uint Blast;
        public int IsReplace;
        public ulong UID;
        public Vector3 BlastDir;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Ver = reader.ReadUInt32();
            Blast = reader.ReadUInt32();
            UID = reader.ReadUInt64();
            BlastDir = reader.ReadVector3();
            IsReplace = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            base.WriteElementData(writer, version);

            writer.Write(Ver);
            writer.Write(Blast);
            writer.Write(UID);
            writer.Write(BlastDir.x);
            writer.Write(BlastDir.y);
            writer.Write(BlastDir.z);
            writer.Write(IsReplace);
        }
    }
}
