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

        internal override void ReadEffectData(DataReader reader, bool alt)
        {
            base.ReadEffectData(reader, alt);

            Start = reader.ReadInt32();
            End = reader.ReadInt32();

            CuesheetID = reader.ReadUInt16();
            SoundID = reader.ReadUInt16();
        }

        internal override void WriteEffectData(DataWriter writer, bool alt)
        {
            base.WriteEffectData(writer, alt);

            writer.Write(Start);
            writer.Write(End);

            writer.Write(CuesheetID);
            writer.Write(SoundID);
        }
    }
}
