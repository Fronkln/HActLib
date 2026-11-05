using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y0, 109)]
    [ElementID(Game.YK1, 109)]
    [ElementID(Game.FOTNS, 109)]
    public class OEDOF2 : OEBaseEffect
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;

        public float FarBefore;
        public float NearBefore;
        public float FarAfter;
        public float NearAfter;
        public float FStopBefore;
        public float FStopAfter;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();

            FarBefore = reader.ReadSingle();
            NearBefore = reader.ReadSingle();

            FarAfter = reader.ReadSingle();
            NearAfter = reader.ReadSingle();

            FStopBefore = reader.ReadSingle();
            FStopAfter = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(FarBefore);
            writer.Write(NearBefore);

            writer.Write(FarAfter);
            writer.Write(NearAfter);

            writer.Write(FStopBefore);
            writer.Write(FStopAfter);
        }
    }
}
