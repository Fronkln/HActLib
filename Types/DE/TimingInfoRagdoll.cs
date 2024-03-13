using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{

    public class TimingInfoRagdoll
    {
        public uint Param;

        public float VelocityX;
        public float VelocityY;
        public float VelocityZ;

        public int Flag;

        public float AngularX;
        public float AngularY;
        public float AngularZ;

        internal void Read(DataReader reader)
        {
            Param = reader.ReadUInt32();

            VelocityX = reader.ReadSingle();
            VelocityY = reader.ReadSingle();
            VelocityZ = reader.ReadSingle();

            Flag = reader.ReadInt32();

            AngularX = reader.ReadSingle();
            AngularY = reader.ReadSingle();
            AngularZ = reader.ReadSingle();
        }

        internal void Write(DataWriter writer)
        {
            writer.Write(Param);

            writer.Write(VelocityX);
            writer.Write(VelocityY);
            writer.Write(VelocityZ);

            writer.Write(Flag);

            writer.Write(AngularX);
            writer.Write(AngularY);
            writer.Write(AngularZ);
        }
    }
}
