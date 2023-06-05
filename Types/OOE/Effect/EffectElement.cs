using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class EffectElement : EffectBase
    {
        public float Start;
        public float End;

        public int ElementFlags;
        public int ElementUnk2;

        public EffectID EffectID2;

        internal override void ReadEffectData(DataReader reader, bool alt)
        {
            base.ReadEffectData(reader, alt);


            if (!alt)
            {
                Start = reader.ReadSingle();
                End = reader.ReadSingle();

                ElementFlags = reader.ReadInt32();
                ElementUnk2 = reader.ReadInt32();

                EffectID2 = (EffectID)reader.ReadUInt32();
            }
        }

        internal override void WriteEffectData(DataWriter writer)
        {
            writer.Write(Start);
            writer.Write(End);

            writer.Write(ElementFlags);
            writer.Write(ElementUnk2);

            writer.Write((uint)EffectID2);
        }
    }
}
