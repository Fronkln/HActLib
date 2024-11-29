using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActEffectParticle : YActEffect
    {
        public int Particle;

        internal override void ReadEffectData(DataReader reader)
        {
            Particle = reader.ReadInt32();
        }

        internal override void WriteEffectData(DataWriter writer)
        {
            writer.Write(Particle);
        }
    }
}
