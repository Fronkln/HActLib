using System;
using System.Linq;
using System.Collections.Generic;
using Yarhl.FileFormat;
using Yarhl.IO;
using System.ComponentModel.DataAnnotations;

namespace HActLib
{

    //Character Data: Aligned
    //Special Elements Data: Aligned
    //Character Data 2: Being written TWICE?!!!!
    //String Table: Unaligned

    public class CSVWriter : IConverter<CSV, BinaryFormat>
    {
        private DataWriter m_writer;

        private Dictionary<string, int> m_stringTable = new Dictionary<string, int>();
        private long m_stringTableAddr = 0; //i bet you will forget to cast this to int while writing

        private Dictionary<CSVHAct, int> m_csvEntries = new Dictionary<CSVHAct, int>();
        
        private Dictionary<CSVHAct, int> m_csvCharactersStart = new Dictionary<CSVHAct, int>();
        private Dictionary<CSVCharacter, int> m_csvCharacters = new Dictionary<CSVCharacter, int>();
        
        private Dictionary<CSVHAct, int> m_csvSpecialNodesStart = new Dictionary<CSVHAct, int>();
        private Dictionary<CSVHActEvent, int> m_csvSpecialNodes = new Dictionary<CSVHActEvent, int>();

        private Dictionary<CSVHAct, int> m_csvSection4Start = new Dictionary<CSVHAct, int>();
        private Dictionary<CSVSection4, int> m_csvSection4 = new Dictionary<CSVSection4, int>();

        private Dictionary<CSVHAct, int> m_csvCharacterExtraDataStart = new Dictionary<CSVHAct, int>();
        private Dictionary<CSVCharacterExtraData, int> m_csvCharacterExtraDatas = new Dictionary<CSVCharacterExtraData, int>();
        

        public BinaryFormat Convert(CSV csv)
        {
            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            m_writer = writer;

            writer.DefaultEncoding = System.Text.Encoding.GetEncoding(932);
            writer.Endianness = EndiannessMode.BigEndian;

            writer.Write(new byte[] { 0x54, 0x43, 0x41, 0x48 });
            writer.Write(0x102);
            writer.Write(20081217);
            writer.WriteTimes(0, 4);

            long pointersRegion = writer.Stream.Position;
            writer.WriteTimes(0, 48);


            long entriesRegion = writer.Stream.Position;

            //Same deal with TEV. We write the incomplete data first, then complete it over multiple passes.
            //We are starting with just the entry data with no regard for pointers and other sections.

            foreach(var entry in csv.Entries)
            {
                m_csvEntries.Add(entry, (int)writer.Stream.Position);
                writer.WriteTimes(0, 96);
               // entry.Write(writer);
            }

            long charactersRegion = writer.Stream.Position;

            //Write character information with no regard for pointers and other sections.
            foreach(var entry in csv.Entries)
            {
                m_csvCharactersStart[entry] = (int)writer.Stream.Position;

                foreach(var chara in entry.Characters)
                {
                    m_csvCharacters[chara] = (int)writer.Stream.Position;
                    chara.Write(writer);
                }
            }
            

            long specialNodesRegion = writer.Stream.Position;

            //Write special element information with no regard for pointers and other sections.
            foreach (var entry in csv.Entries)
            {
                m_csvSpecialNodesStart[entry] = (int)writer.Stream.Position;

                foreach (var special in entry.SpecialNodes)
                {
                    m_csvSpecialNodes[special] = (int)writer.Stream.Position;
                    special.WriteData(writer);

                    if(special.UnreadData != null)
                        writer.Write(special.UnreadData);
                }
            }
        
            long region4Area = writer.Stream.Position;


            //Write section 4 with no regard for pointers and other sections.
            foreach (var entry in csv.Entries)
            {
                m_csvSection4Start[entry] = (int)writer.Stream.Position;

                foreach (var special in entry.Section4)
                {
                    m_csvSection4[special] = (int)writer.Stream.Position;
                    writer.WriteTimes(0, 64);
                }
            }

            long characterExtraDataRegion = writer.Stream.Position;

            //Write character extra data information with no regard for pointers and other sections.
            //Serious problem: Writes string table for some reason : i == 252
            


            for(int i = 0; i < csv.Entries.Count; i++)
            {
                CSVHAct entry = csv.Entries[i];

                m_csvCharacterExtraDataStart[entry] = (int)writer.Stream.Position;

                foreach (var chara in entry.Characters)
                    foreach (var extraDat in chara.UnknownExtraData)
                    {
                        m_csvCharacterExtraDatas[extraDat] = (int)writer.Stream.Position;
                        extraDat.Write(writer);
                    }
            }

            long m_stringTableRegion = writer.Stream.Position;
            m_stringTableAddr = writer.Stream.Position;

            //Go back to hact entries. write everything.
            foreach (var entry in csv.Entries)
            {
                writer.Stream.Seek(m_csvEntries[entry], SeekMode.Start);
                
                writer.Write(WriteToStringTable(entry.Name));
                writer.Write(entry.ID);
                writer.Write(entry.Unk1);
                writer.Write(entry.Unk2);
                writer.Write(entry.Flags);
                writer.Write(entry.UnkSection1);
                writer.Write(m_csvCharactersStart[entry]);
                writer.Write(entry.Characters.Count);
                writer.Write(m_csvSpecialNodesStart[entry]);
                writer.Write(entry.SpecialNodes.Count);
                writer.Write(m_csvSection4Start[entry]);
                writer.Write(entry.Section4.Count);

                //Go back to characters. link things.
                foreach (var chara in entry.Characters)
                {
                    writer.Stream.Seek(m_csvCharacters[chara]);
                    writer.Write(WriteToStringTable(chara.Name));
                    writer.Write(WriteToStringTable(chara.ModelOverride));
                    writer.Stream.Position += 64;
                    writer.Write(m_csvCharacterExtraDataStart[entry]);
                    writer.Write(chara.UnknownNum);
                }

                //Go back to special nodes. link things.
                foreach (var specialNode in entry.SpecialNodes)
                {
                    writer.Stream.Seek(m_csvSpecialNodes[specialNode]);
                    long start = writer.Stream.Position;

                    specialNode.WriteData(writer);
                    writer.Stream.Seek(start);
                    writer.Write(WriteToStringTable(specialNode.Type));
                    writer.Stream.Seek(start + 32);

                    foreach (string str in specialNode.Resources)
                        writer.Write(WriteToStringTable(str));

                    writer.Write(WriteToStringTable(specialNode.UnknownResource));
                }

                //Go back to section 4. Link things.
                foreach (var sec4 in entry.Section4)
                {
                    writer.Stream.Seek(m_csvSection4[sec4]);
                    writer.Write(sec4.Unknown);

                    foreach (string str in sec4.Resources)
                        writer.Write(WriteToStringTable(str));

                    writer.Write(sec4.Unknown2);
                }
            }

            //Go back to extra character data, take care of pointers.
            foreach(var kv in m_csvCharacterExtraDatas)
            {
                writer.Stream.Seek(kv.Value + 4);

                foreach (string str in kv.Key.Resources)
                    writer.Write(WriteToStringTable(str));
            }

            //Go back to header. Write everything
            writer.Stream.Seek(pointersRegion, SeekMode.Start);
            writer.Write((int)entriesRegion);
            writer.Write(csv.Entries.Count);

            
            writer.Write((int)charactersRegion);
            writer.Write(m_csvCharacters.Count);
            writer.Write((int)specialNodesRegion);
            writer.Write(m_csvSpecialNodes.Count);
            writer.Write((int)region4Area);
            writer.Write(m_csvSection4.Count);
            writer.Write((int)characterExtraDataRegion);
            writer.Write(m_csvCharacterExtraDatas.Count);
            writer.Write((int)(m_stringTableRegion - 64));
            writer.Write((int)m_stringTableRegion);

            writer.Stream.Seek(writer.Stream.Length);
            writer.Align(32);

            return binary;
        }

        //Returns the address of the written value or returns the pre-existing one
        private int WriteToStringTable(string value)
        {
            if (string.IsNullOrEmpty(value))
                value = "";

            if (m_stringTable.ContainsKey(value))
                return m_stringTable[value];

            int valuePos = 0; 

            m_writer.Stream.RunInPosition(delegate
            {
                valuePos = (int)m_writer.Stream.Position;
                m_writer.Write(value, true);
                m_stringTableAddr = m_writer.Stream.Position;
            }, m_stringTableAddr, SeekMode.Start);

            m_stringTable[value] = valuePos;

            return valuePos;
        }
    }
}
