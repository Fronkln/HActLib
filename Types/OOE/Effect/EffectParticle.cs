using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class EffectParticle : EffectElement
    {
        public uint ParticleID;

        internal override void ReadEffectData(DataReader reader)
        {
            base.ReadEffectData(reader);

            ParticleID = reader.ReadUInt32();
        }

        internal override void WriteEffectData(DataWriter writer)
        {
            base.WriteEffectData(writer);

            writer.Write(ParticleID);
        }
    }
}
