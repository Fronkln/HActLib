using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class EffectBase : ITEVObject
    {
        public Guid Guid;
        public EffectID ElementKind;
        public uint Unk2;
        public uint Unk3;

        public byte[] Data;
        public byte[] OptionalUnk = null; //32 bytes, doesnt always exist

        public override string ToString()
        {
            return GetName();
        }

        public string GetName()
        {
            return "Element " + ElementKind;
        }


        /// <summary>
        /// Reads base data, and returns the size of the effect.
        /// </summary>
        internal uint ReadBaseData(DataReader reader)
        {
            Guid = new Guid(reader.ReadBytes(16));
            ElementKind = (EffectID)reader.ReadUInt32();
            uint dataSize = reader.ReadUInt32();

            Unk2 = reader.ReadUInt32();
            Unk3 = reader.ReadUInt32();

            return dataSize;
        }

        internal virtual void ReadEffectData(DataReader reader)
        {

        }

        internal void WriteBaseData(DataWriter writer)
        {
            writer.Write(Guid.ToByteArray());
            writer.Write((uint)ElementKind);
            writer.Write(0); //the size will be written by the algoritm authomatically

            writer.Write(Unk2);
            writer.Write(Unk3);
        }

        internal virtual void WriteEffectData(DataWriter writer)
        {

        }


        internal static EffectBase ReadFromMemory(DataReader reader)
        {
            EffectID id = EffectID.Dummy;
            reader.Stream.RunInPosition(() => id = (EffectID)reader.ReadInt32(), 16, SeekMode.Current);

            EffectBase set3 = CreateEffectObject(id);

            uint effectSize = set3.ReadBaseData(reader);
            long endAddress = reader.Stream.Position + effectSize;

            set3.ReadEffectData(reader);

            int unreadBytes = (int)(endAddress - reader.Stream.Position);

            set3.Data = reader.ReadBytes((int)(endAddress - reader.Stream.Position));

            bool hasOptionalUnk = false;

            reader.Stream.RunInPosition(delegate
            {
                if (reader.ReadInt32() == -1)
                    hasOptionalUnk = true;

            }, 0, SeekMode.Current);

            if (hasOptionalUnk)
                set3.OptionalUnk = reader.ReadBytes(32);

            return set3;
        }

        internal void WriteToStream(DataWriter writer)
        {
            //EffectBase set = tev.Effects[i];
            //h_set3Addresses[set] = writer.Stream.Position;

            long startAddr = writer.Stream.Position; 

            WriteBaseData(writer);
            long dataStart = writer.Stream.Position;
            WriteEffectData(writer);

            //Write unprocessed data
            writer.Write(Data);

            long size = writer.Stream.Position - dataStart;
            writer.Stream.RunInPosition(() => writer.Write((uint)size), startAddr + 20, SeekMode.Start);

            if (OptionalUnk != null)
                writer.Write(OptionalUnk);
        }

        internal static EffectBase CreateEffectObject(EffectID id)
        {
            switch (id)
            {
                default:
                    if ((int)id < 1000)
                        return new EffectBase();
                    else
                        return new EffectElement();
                case EffectID.Sound:
                    return new EffectSound();
                case EffectID.Particle:
                    return new EffectParticle();
            }
        }
    }
}
