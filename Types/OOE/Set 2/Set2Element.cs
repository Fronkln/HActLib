using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public virtual bool ShouldWriteEffectData => true;

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

            int unreadBytes = (int)(end - reader.Stream.Position);
            
            if (reader.Stream.Position < end)
                Debug.WriteLine("Set2: Read " + unreadBytes + " less than expected! " + Effect.ToString());
            else if(reader.Stream.Position > end)
                Debug.WriteLine("Set2: Read " + -unreadBytes + " more than expected! " + Effect.ToString());


            // if (reader.Stream.Position < end)
            Unk2 = reader.ReadBytes(unreadBytes);
            

            //Unk2 = reader.ReadBytes(252);
        }

        internal override void WriteArgs(DataWriter writer, bool alt)
        {
            long start = writer.Stream.Position;
            long end = writer.Stream.Position + 252;

            if(Effect != null && ShouldWriteEffectData)
                Effect.WriteEffectData(writer, alt);

            long afterDataWrite = writer.Stream.Position;

            if (Unk2 != null)
            {
                int difference = (int)(afterDataWrite - start);
                int unk2WriteAmount = Unk2.Length - difference;

                if (unk2WriteAmount > 0)
                {
                    byte[] unk2ToWrite = new byte[unk2WriteAmount];

                    Array.Copy(Unk2, difference, unk2ToWrite, 0, Unk2.Length - difference);
                    writer.Write(unk2ToWrite);
                }
            }

            int unwrittenBytes = (int)(end - writer.Stream.Position);

            if (unwrittenBytes > 0)
                writer.WriteTimes(0, unwrittenBytes);

            if(unwrittenBytes < 0)
                throw new Exception("OVERWRITE! Position: " + start+ " Type: " +  Type  + " " + EffectID + $"({ToString()})");
        }
    }
}
