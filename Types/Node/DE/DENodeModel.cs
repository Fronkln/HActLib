using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class NodeModel : Node
    {
        public PXDHash BoneName = new PXDHash();
        public int UnkOE;
        public byte[] UnkDE = new byte[8];

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint hactVer = inf.version;

            if (hactVer > 10)
            {
                BoneName = reader.Read<PXDHash>();
                
                if (hactVer >= 18)
                   UnkDE = reader.ReadBytes(8);
                else
                {
                    UnkOE = reader.ReadInt32();
                    reader.ReadBytes(12);
                }
            }
            else
            {
                BoneName.Set(Name);
            }
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            if (hactVer > 10)
            {
                writer.WriteOfType(BoneName);

                if (hactVer >= 18)
                    writer.Write(UnkDE);
                else
                {
                    writer.WriteOfType(UnkOE);
                    writer.WriteTimes(0, 12);
                }
            }
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            if (hactVer <= 10)
                return 0x4;

            if (hactVer < 18)
                return 0xC;

            return 10;
        }
    }
}
