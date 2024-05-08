using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class RimflashParamsV3 : RimflashParams
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

        internal override void Read(DataReader reader)
        {
            base.Read(reader);

            Version = reader.ReadUInt32();
            Color0Intensity = reader.ReadSingle();
            Color1Intensity = reader.ReadSingle();
            Color00 = reader.Read<RGB>();
            Color01 = reader.Read<RGB>();
            Color01Fresnel = reader.ReadSingle();
            Color10 = reader.Read<RGB>();
            Color11 = reader.Read<RGB>();
            Color1Fresnel = reader.ReadSingle();
            NoiseCoordinateSize = reader.ReadSingle();
            TurbulenceCoordinateSize = reader.ReadSingle();
            TurbulenceScaleBase = reader.ReadSingle();
            TurbulanceScaleSpd = reader.ReadSingle();
            NormalLerp = reader.ReadSingle();
            Alpha = reader.ReadSingle();
            flow_grav = reader.ReadSingle();
            flow_wx = reader.ReadSingle();
            flow_wy = reader.ReadSingle();
            flow_wz = reader.ReadSingle();
            flow_lx = reader.ReadSingle();
            flow_ly = reader.ReadSingle();
            flow_lz = reader.ReadSingle();
            flow_vz = reader.ReadSingle();
            flow_node_spd = reader.ReadSingle();
            flow_fric = reader.ReadSingle();
            Pattern = reader.ReadInt32();
            AdjustDecompressLuminance = reader.ReadSingle();
            AdjustDecompressLuminanceOpaque = reader.ReadSingle();
            ColorIntensityPower = reader.ReadSingle();
            ChromaPower = reader.ReadSingle();
        }

        internal override void Write(DataWriter writer)
        {
            base.Write(writer);

            writer.Write(Version);
            writer.Write(Color0Intensity);
            writer.Write(Color1Intensity);
            writer.WriteOfType(Color00);
            writer.WriteOfType(Color01);
            writer.Write(Color01Fresnel);
            writer.WriteOfType(Color10);
            writer.WriteOfType(Color11);
            writer.Write(Color1Fresnel);
            writer.Write(NoiseCoordinateSize);
            writer.Write(TurbulenceCoordinateSize);
            writer.Write(TurbulenceScaleBase);
            writer.Write(TurbulanceScaleSpd);
            writer.Write(NormalLerp);
            writer.Write(Alpha);
            writer.Write(flow_grav);
            writer.Write(flow_wx);
            writer.Write(flow_wy);
            writer.Write(flow_wz);
            writer.Write(flow_lx);
            writer.Write(flow_ly);
            writer.Write(flow_lz);
            writer.Write(flow_vz);
            writer.Write(flow_node_spd);
            writer.Write(flow_fric);
            writer.Write(Pattern);
            writer.Write(AdjustDecompressLuminance);
            writer.Write(AdjustDecompressLuminanceOpaque);
            writer.Write(ColorIntensityPower);
            writer.Write(ChromaPower);
        }
    }
}
