using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{ 
    public class DEElementBaseLight : NodeElement
    {
        public Vector3 Position;
        public RGB32 Color;
        public int Kelvin;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Position = reader.ReadVector3();
            Color = new RGB32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
            Kelvin = reader.ReadInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(Position);
            writer.Write(Color);
            writer.Write(Kelvin);
        }
    }
}
