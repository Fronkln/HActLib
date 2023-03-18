using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class DENodeCharacterBehavior : Node
    {
        public uint Flags;
        public GameTick InFrame;
        public GameTick OutFrame;

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Flags = reader.ReadUInt32();
            InFrame = new GameTick(reader.ReadUInt32());
            OutFrame = new GameTick(reader.ReadUInt32());
        }
    }
}
