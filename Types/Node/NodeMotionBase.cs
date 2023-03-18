using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yarhl.IO;

namespace HActLib
{
    public class NodeMotionBase : Node
    {
        public uint Flags;
        public GameTick Start = new GameTick();
        public GameTick End = new GameTick();

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            bool isDE = CMN.IsDE(version);

            if (isDE)
                Flags = reader.ReadUInt32();

            if (version == GameVersion.Y0_K1)
            {
                Start = new GameTick(reader.ReadSingle());
                End = new GameTick(reader.ReadSingle());
            }
            else
            {
                Start = new GameTick(reader.ReadSingle());
                End = new GameTick(reader.ReadSingle());
            }

            reader.ReadBytes(isDE ? 4 : 8);
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            bool isDE = CMN.IsDE(version);

            if (isDE)
                writer.Write(Flags);

            writer.Write(Start.Frame);
            writer.Write(End.Frame);

            writer.WriteTimes(0, isDE ? 4 : 8);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 16;
        }
    }
}
