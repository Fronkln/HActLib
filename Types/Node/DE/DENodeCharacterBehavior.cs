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
        public byte[] dat;
        public uint Flags;
        public GameTick InFrame;
        public GameTick OutFrame;
        public uint BehaviorSet;
        public uint BehaviorGroup;
        public uint BehaviorAction;

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Flags = reader.ReadUInt32();
            InFrame = new GameTick(reader.ReadUInt32());
            OutFrame = new GameTick(reader.ReadUInt32());
            BehaviorSet = reader.ReadUInt32();
            BehaviorGroup = reader.ReadUInt32();
            BehaviorAction = reader.ReadUInt32();
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            writer.Write(Flags);
            writer.Write(InFrame.Tick);
            writer.Write(OutFrame.Tick);
            writer.Write(BehaviorSet);
            writer.Write(BehaviorGroup);
            writer.Write(BehaviorAction);
        }
    }
}
