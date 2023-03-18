using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class OENodeCharacter : Node
    {
        public uint Height;
        public int Unk1;
        public int Unk2;
        public int Unk3;

        public OENodeCharacter()
        {
            Category = AuthNodeCategory.Character;
        }
        
        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Height = reader.ReadUInt32();

            Unk1 = reader.ReadInt32();
            Unk2 = reader.ReadInt32();
            Unk3 = reader.ReadInt32();
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(Height);
          
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Unk3);         
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 16;
        }
    }
}
