using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParamsV4 : RimflashParamsV3
    {
        public float UnknownV4_1 { get; set; }


        internal override void Read(DataReader reader)
        {
            base.Read(reader);

            UnknownV4_1 = reader.ReadSingle();
        }
    }
}
