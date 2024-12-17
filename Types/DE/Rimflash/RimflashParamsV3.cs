using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParamsV3 : RimflashParamsV2
    {
        public float ColorIntensityPower { get; set; } = 0.5f;
        public float ChromaPower { get; set; } = 1f;

        internal override void Read(DataReader reader)
        {
            base.Read(reader);
            ColorIntensityPower = reader.ReadSingle();
            ChromaPower = reader.ReadSingle();
        }

        internal override void Write(DataWriter writer)
        {
            base.Write(writer);

            writer.Write(ColorIntensityPower);
            writer.Write(ChromaPower);
        }
    }
}
