using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParamsV5 : RimflashParamsV4
    {
        public float UnknownV5_1 { get; set; } = 1f;


        internal override void Read(DataReader reader)
        {
            base.Read(reader);

            UnknownV5_1 = reader.ReadSingle();
        }

        internal override void Write(DataWriter writer)
        {
            base.Write(writer);

            writer.Write(UnknownV5_1);
        }
    }
}
