using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class EffectSound : EffectBase
    {
        public int Start;
        public int End;

        public ushort CuesheetID;
        public ushort SoundID;

        public int Unknown1 = 1;
        public int Unknown2 = 0;
        public int Unknown3 = 0;
        public int Unknown4 = 0;
        public int Unknown5 = 0;
        public int Unknown6 = 0;

        public EffectSound() : base()
        {
            ElementKind = EffectID.Sound;   
        }

        internal override void ReadEffectData(DataReader reader, bool alt)
        {
            base.ReadEffectData(reader, alt);

            Start = reader.ReadInt32();
            End = reader.ReadInt32();

            CuesheetID = reader.ReadUInt16();
            SoundID = reader.ReadUInt16();

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
        }

        internal override void WriteEffectData(DataWriter writer, bool alt)
        {
            base.WriteEffectData(writer, alt);

            writer.Write(Start);
            writer.Write(End);

            writer.Write(CuesheetID);
            writer.Write(SoundID);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
        }
    }
}
