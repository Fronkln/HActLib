using System;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActEffectSound : YActEffect
    {
        public ushort CuesheetID;
        public ushort SoundID;

        internal override void ReadEffectData(DataReader reader)
        {
            CuesheetID = reader.ReadUInt16();
            SoundID = reader.ReadUInt16();
        }

        internal override void WriteEffectData(DataWriter writer)
        {
            writer.Write(CuesheetID);
            writer.Write(SoundID);
        }
    }
}
