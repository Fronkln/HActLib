using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    //Set2 element is just EffectBase but with padded data
    //Aint that some shit!
    public class Set2Element : Set2
    {
        public EffectBase Effect;

        public override string GetName()
        {
            return "Element " + EffectID;
        }

        internal override void ReadArgs(DataReader reader)
        {
            long start = reader.Stream.Position;
            long end = reader.Stream.Position + 252;


            Effect = EffectBase.CreateEffectObject(EffectID);
            Effect.ReadEffectData(reader, true);
            Effect.ElementKind = EffectID;
            reader.ReadBytes((int)(end - reader.Stream.Position));
            

            //Unk2 = reader.ReadBytes(252);
        }

        internal override void WriteArgs(DataWriter writer)
        {
            long start = writer.Stream.Position;
            long end = writer.Stream.Position + 252;

            if(Effect != null)
                Effect.WriteEffectData(writer);

            writer.WriteTimes(0, end - writer.Stream.Position);
        }
    }
}
