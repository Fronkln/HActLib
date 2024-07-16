using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public byte[] Data = new byte[0];
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

        internal virtual void ReadEffectData(DataReader reader, bool alt)
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

        internal virtual void WriteEffectData(DataWriter writer, bool alt)
        {

        }


        internal static (EffectBase, bool) ReadFromMemory(DataReader reader, bool alt)
        {
            EffectID id = EffectID.Dummy;
            reader.Stream.RunInPosition(() => id = (EffectID)reader.ReadInt32(), 16, SeekMode.Current);

            EffectBase set3 = CreateEffectObject(id);

            uint effectSize = set3.ReadBaseData(reader);
            long endAddress = reader.Stream.Position + effectSize;

            set3.ReadEffectData(reader, alt);

            int unreadBytes = (int)(endAddress - reader.Stream.Position);

            if (reader.Stream.Position < endAddress)
            {
                Debug.WriteLine("Effect: Read " + unreadBytes + " less than expected! " + set3.ToString());
                set3.Data = reader.ReadBytes((int)(unreadBytes));
            }

            if(reader.Stream.Position > endAddress)
            {
                Debug.WriteLine("Effect  Read " + -unreadBytes + " more than expected! " + set3.ToString());
                reader.Stream.Position = endAddress;
            }

            bool isLastElement = true;

            reader.Stream.RunInPosition(delegate
            {
                for (int i = 0; i < 4; i++)
                {
                    if (reader.ReadInt32() != -1)
                    {
                        isLastElement = false;
                        break;
                    }
                }
            }, 0, SeekMode.Current);


            if (isLastElement)
                reader.Stream.Position += 32;

            return (set3, isLastElement);
        }

        internal void WriteToStream(DataWriter writer, bool alt)
        {
            //EffectBase set = tev.Effects[i];
            //h_set3Addresses[set] = writer.Stream.Position;

            long startAddr = writer.Stream.Position; 

            WriteBaseData(writer);
            long dataStart = writer.Stream.Position;
            WriteEffectData(writer, alt);

            //Write unprocessed data
            writer.Write(Data);

            long size = writer.Stream.Position - dataStart;
            uint size32 = Convert.ToUInt32(size);
            writer.Stream.RunInPosition(() => writer.Write(size32), startAddr + 20, SeekMode.Start);
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
