using HActLib.OOE;
using Yarhl.IO;

namespace HActLib
{
    public class MepEffectY3 : MepEffect
    {
        public EffectBase Effect;

        internal override void Read(DataReader reader, MEPVersion version)
        {
           Effect = EffectBase.ReadFromMemory(reader, false).Item1;
        }

        internal override void Write(DataWriter writer, MEPVersion version)
        {
            Effect.WriteToStream(writer, false);
        }
    }
}
