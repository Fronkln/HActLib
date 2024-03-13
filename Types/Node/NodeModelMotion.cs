using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yarhl.IO;

namespace HActLib
{
    public class NodeModelMotion : NodeMotionBase
    {
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
                Start = new GameTick(reader.ReadUInt32());              
                End = new GameTick(reader.ReadUInt32());
            }

            reader.ReadBytes(isDE ? 4 : 8);
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            WriteCoreData(writer, version, hactVer);

            bool isDE = CMN.IsDE(version);

            if (isDE)
                writer.Write(Flags);

            if (!isDE)
            {
                writer.Write(Start.Frame);
                writer.Write(End.Frame);
            }
            else
            {
                writer.Write(Start.Tick);
                writer.Write(End.Tick);
            }

            writer.WriteTimes(0, isDE ? 4 : 8);
        }

    }
}
