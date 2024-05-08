using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x1D)]
    [ElementID(Game.YK2, 0x1D)]
    [ElementID(Game.JE, 0x1D)]
    [ElementID(Game.YLAD, 0x1C)]
    [ElementID(Game.LJ, 0x1C)]
    [ElementID(Game.LAD7Gaiden, 0x1C)]
    [ElementID(Game.LADIW, 0x1C)]
    public class DEElementFlowdust : NodeElement
    {
        public uint FlowVersion;
        public int SetParamOffset;
        public float Radius;
        public float Amount;

        private byte[] unreadData;

        public uint SetParamVersion;

        public RGB Col0;
        public float Col0I;
        public RGB Col1;
        public float Col1I;
        public float Lifetime;
        public float LifetimeRange;
        public float Radius2;
        public float RadiusRange;
        public float FlowFric;
        public float FlowFricRange;
        public float Grav;
        public float GravRange;
        public float ColIPow = 1.5f;
        public float Chroma = 3;
        public float FadePower = 1;

        public bool ParameterFlowdust = false;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            long headerStart = reader.Stream.Position;

            FlowVersion = reader.ReadUInt32();
            SetParamOffset = reader.ReadInt32();
            Radius = reader.ReadSingle();
            Amount = reader.ReadSingle();

            long paramOffset = (SetParamOffset * 4);
            long parameterAddr = headerStart + paramOffset;

            int bytesToRead = (int)(parameterAddr - reader.Stream.Position);

            if (bytesToRead > 0)
            {
                unreadData = reader.ReadBytes(bytesToRead);

                SetParamVersion = reader.ReadUInt32();

                Col0 = reader.Read<RGB>();
                Col0I = reader.ReadSingle();
                Col1 = reader.Read<RGB>();
                Col1I = reader.ReadSingle();

                Lifetime = reader.ReadSingle();
                LifetimeRange = reader.ReadSingle();
                Radius2 = reader.ReadSingle();
                RadiusRange = reader.ReadSingle();
                FlowFric = reader.ReadSingle();
                FlowFricRange = reader.ReadSingle();
                Grav = reader.ReadSingle();
                GravRange = reader.ReadSingle();

                //YLAD and above
                if (SetParamVersion > 0)
                {
                    ColIPow = reader.ReadSingle();
                    Chroma = reader.ReadSingle();
                    FadePower = reader.ReadSingle();
                }
            }
            else
                ParameterFlowdust = true;

            Debug.WriteLine("parameter addr " + parameterAddr);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(FlowVersion);
            writer.Write(SetParamOffset);
            writer.Write(Radius);
            writer.Write(Amount);

            if (!ParameterFlowdust)
            {
                writer.Write(unreadData);

                writer.Write(version > GameVersion.DE1 ? 1 : 0);

                writer.WriteOfType(Col0);
                writer.Write(Col0I);
                writer.WriteOfType(Col1);
                writer.Write(Col1I);

                writer.Write(Lifetime);
                writer.Write(LifetimeRange);
                writer.Write(Radius2);
                writer.Write(RadiusRange);
                writer.Write(FlowFric);
                writer.Write(FlowFricRange);
                writer.Write(Grav);
                writer.Write(GravRange);

                if (version > GameVersion.DE1)
                {
                    writer.Write(ColIPow);
                    writer.Write(Chroma);
                    writer.Write(FadePower);
                }
            }
        }
    }
}
