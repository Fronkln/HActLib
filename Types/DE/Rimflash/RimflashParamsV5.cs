using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParamsV3
    {
        public uint Version { get; set; }
        public float Color0Intensity { get; set; }
        public float Color1Intensity { get; set; }
        public RGB Color00 { get; set; }
        public RGB Color01 { get; set; }
        public float Color01Fresnel { get; set; }
        public RGB Color10 { get; set; }
        public RGB Color11 { get; set; }
        public float Color1Fresnel { get; set; }
        public float NoiseCoordinateSize { get; set; }
        public float TurbulenceCoordinateSize { get; set; }
        public float TurbulenceScaleBase { get; set; }
        public float TurbulanceScaleSpd { get; set; }
        public float NormalLerp { get; set; }
        public float Alpha { get; set; }
        public float flow_grav { get; set; }
        public float flow_wx { get; set; }
        public float flow_wy { get; set; }
        public float flow_wz { get; set; }
        public float flow_lx { get; set; }
        public float flow_ly { get; set; }
        public float flow_lz { get; set; }
        public float flow_vz { get; set; }
        public float flow_node_spd { get; set; }
        public float flow_fric { get; set; }
        public int Pattern { get; set; }
        public float AdjustDecompressLuminance { get; set; }
        public float AdjustDecompressLuminanceOpaque { get; set; }
        public float ColorIntensityPower { get; set; }
        public float ChromaPower { get; set; }
    }
}
