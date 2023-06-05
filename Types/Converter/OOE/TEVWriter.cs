using System;
using System.Collections.Generic;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;
using System.Data;

namespace HActLib
{
    public class TEVWriter : IConverter<TEV, BinaryFormat>
    {
        public BinaryFormat Convert(TEV tev)
        {
            Dictionary<ObjectBase, long> h_set1Addresses = new Dictionary<ObjectBase, long>();
            Dictionary<Set2, long> h_set2Addresses = new Dictionary<Set2, long>();
            Dictionary<EffectBase, long> h_set3Addresses = new Dictionary<EffectBase, long>();
            Dictionary<string, long> h_strTableAddresses = new Dictionary<string, long>();

            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.DefaultEncoding = System.Text.Encoding.GetEncoding(932);

            writer.Endianness = EndiannessMode.BigEndian;

            //Write core information about hact_tev
            writer.Write(new byte[] { 0x54, 0x43, 0x41, 0x48 });
            writer.Write(0x102);
            writer.Write(20081118);
            writer.WriteTimes(0, 4);

            //We will return to this later once we are ready to finish it up
            //Header offset = 0x16
            long pointersArea = writer.Stream.Position;

            //Fill it with empty bytes for now
            writer.WriteTimes(0, 128);

            long set1Start = writer.Stream.Position;

            ObjectBase[] objects = tev.AllObjects;

            //Writing will be done in multiple passes. We will process nodes multiple times as we need it.
            //Pass 1: Write set 1 data without any regard for pointers.
            for (int i = 0; i < objects.Length; i++)
            {
                ObjectBase set = objects[i];
                h_set1Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer);
            }

            long set2Start = writer.Stream.Position;

            Set2[] set2 = tev.AllSet2;

            //Pass 1: Write set 2 data without any regard for pointers.
            for (int i = 0; i < set2.Length; i++)
            {
                Set2 set = set2[i];
                h_set2Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer);
            }

            long set3Start = writer.Stream.Position;

            EffectBase[] effects = tev.AllEffects;

            //Write set 3 data.
            for (int i = 0; i < effects.Length; i++)
            {
                EffectBase set = effects[i];
                h_set3Addresses[set] = writer.Stream.Position;

                set.WriteToStream(writer);

                if(i == effects.Length - 1)
                {
                    writer.WriteTimes(255, 16);
                    writer.WriteTimes(0, 16);
                }    
            }

            int unkPtr1 = (int)writer.Stream.Position;
            //writer.Align(512);

            int setData1Start = (int)writer.Stream.Position;

            //Write set 1 data section 1 (and resources which is part of data)

            foreach (ObjectBase set in objects)
                set._InternalInfo.DataPtr1 = set.WriteData(writer, set.UnkFloatDats[0]);

            writer.WriteTimes(0, tev.TEVHeader.DataPadding);

            int setData2Start = (int)writer.Stream.Position;

            foreach (ObjectBase set in objects)
                set._InternalInfo.DataPtr2 = set.WriteData(writer, set.UnkFloatDats[1]);

            int stringsStart = (int)writer.Stream.Position;

            writer.Endianness = EndiannessMode.LittleEndian;
            writer.Write(tev.CuesheetID);
            writer.Endianness = EndiannessMode.BigEndian;
                
            int unkPtr2 = (int)writer.Stream.Position;

            int stringTableStart = (int)writer.Stream.Position;

            foreach (ObjectBase set in objects)
            {
                int nodeIdx = Array.IndexOf(objects, set);

                set._InternalInfo.Parent = (int)(set.Parent == null ? -1 : h_set1Addresses[set.Parent]);
                set._InternalInfo.PreviousNode = (int)(set.PreviousNode == null ? -1 : h_set1Addresses[set.PreviousNode]);
                set._InternalInfo.NextNode = (int)(set.NextNode == null ? -1 : h_set1Addresses[set.NextNode]);
                set._InternalInfo.NextMainNode = (int)(set.NextMainNode == null ? -1 : h_set1Addresses[set.NextMainNode]);

                set._InternalInfo.StringTables = set.WriteStringTable(writer, h_strTableAddresses);
                set._InternalInfo.Set3Ptr = (set.Set3Object != null ?  (int)h_set3Addresses[set.Set3Object] : -1);
                     
                if (set.Set2Object != null)
                    set.Set2Object._InternalInfo.resourcePtr = set.Set2Object.WriteResource(writer);
            }

            writer.Stream.Seek(set1Start, SeekMode.Start);

            for (int i = 0; i < objects.Length; i++)
            {
                ObjectBase set = objects[i];
                h_set1Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer);
            }

            for (int i = 0; i < set2.Length; i++)
            {
                Set2 set = set2[i];
                h_set2Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer);
            }

            //finish header
            writer.Stream.Seek(pointersArea, SeekMode.Start);

            writer.Write((int)set1Start);
            writer.Write(objects.Length);

            writer.Write((int)set2Start);
            writer.Write(set2.Length);

            writer.Write((int)set3Start);
            writer.Write(setData1Start);

            writer.Write(objects.Length);

            
            writer.Write(setData2Start);

            writer.Write(tev.TEVHeader.UnkCount2);
            writer.Write(stringsStart);

            writer.Write(tev.TEVHeader.UnkCount3);
            writer.Write(setData2Start - tev.TEVHeader.StringTableOffset);


            writer.Write(tev.TEVHeader.UnkVal1);
            writer.Write(tev.TEVHeader.UnkVal2);
            writer.Write(tev.TEVHeader.UnkVal3);
            writer.Write(tev.TEVHeader.UnkVal4);

            writer.Write(tev.TEVHeader.UnkRegion1);
            writer.Write(tev.TEVHeader.UnkRegion2);

            writer.Write(stringsStart + 4);

            //writer.Write(unkPtr1);

            writer.Stream.Seek(writer.Stream.Length - 1);
            writer.Align(16);
            

            return binary;
        }
    }
}
