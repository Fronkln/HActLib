﻿using Yarhl.IO;
using Yarhl.FileFormat;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

namespace HActLib
{
    public class CSVReader : IConverter<BinaryFormat, CSV>
    {
        private DataReader reader = null;
        private CSV csv = null;

        private Dictionary<CSVCharacter, int> m_extraDatPtr = new Dictionary<CSVCharacter, int>();

        public CSV Convert(BinaryFormat format)
        {
            reader = new DataReader(format.Stream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };
            csv = new CSV();

            //Header
            string magic = reader.ReadString(4);

            if (magic != "TCAH" && magic != "HACT") //TCAH in Y3 HACT in Y4
                throw new System.Exception("Not HAct CSV");

            reader.ReadBytes(4);
            csv.RevisionDate = reader.ReadInt32();
            reader.ReadBytes(4);

            //HAct entries
            uint entriesPointer = reader.ReadUInt32();
            uint entriesCount = reader.ReadUInt32();

            //HAct entry character data
            uint characterDataPointer = reader.ReadUInt32();
            uint characterDataCount = reader.ReadUInt32();

            //HAct entry HE_ node data later used in hact_tev.bin
            uint specialElementPointer = reader.ReadUInt32();
            uint specialElementCount = reader.ReadUInt32();

            //HAct entry unknown data
            uint section4Pointer = reader.ReadUInt32();
            uint section4Count = reader.ReadUInt32();

            //There are two more sections, but we do not need them to fully read the CSV.
            reader.ReadBytes(16);

            reader.Stream.Seek(entriesPointer, SeekMode.Start);

            for (int i = 0; i < entriesCount; i++)
            {
                CSVHAct hactEntry = new CSVHAct();
                hactEntry.Name = ReadStringPointer(reader.ReadInt32());
                hactEntry.ID = reader.ReadUInt32();
                hactEntry.Unk1 = reader.ReadInt32();
                hactEntry.Unk2 = reader.ReadInt32();
                hactEntry.Flags = reader.ReadInt32();
                hactEntry.UnkSection1 = reader.ReadBytes(44);

                int hactCharacterDataPointer = reader.ReadInt32();
                uint hactCharacterDataCount = reader.ReadUInt32();

                int hactSpecialElementDataPointer = reader.ReadInt32();
                uint hactSpecialElementCount = reader.ReadUInt32();

                int hactSection4DataPointer = reader.ReadInt32();
                int hactSection4DataCount = reader.ReadInt32();

                //Padding
                reader.ReadBytes(8);

                long entryEnd = reader.Stream.Position;

                //Read HAct entry character data
                reader.Stream.Seek(hactCharacterDataPointer, SeekMode.Start);
                for (int j = 0; j < hactCharacterDataCount; j++)
                {
                    CSVCharacter character = new CSVCharacter();
                    character.Read(reader);
                    int unkDatPointer = reader.ReadInt32();
                    int unkDatCount = reader.ReadInt32();

                    character.UnknownNum = unkDatCount;

                    m_extraDatPtr[character] = unkDatPointer;

                    hactEntry.Characters.Add(character);
                }

                //Read HAct special element data
                reader.Stream.Seek(hactSpecialElementDataPointer, SeekMode.Start);
                for (int j = 0; j < hactSpecialElementCount; j++)
                {
                    //HE_GAUGE_00, HE_DAMAGE_99 etc...
                    string type = ReadStringPointer(reader.ReadInt32());
                    string[] typeSplit = type.Split('_');

                    //Execute creation and read code here...
                    CSVHActEvent hactEvent = null;

                    switch(typeSplit[1])
                    {
                        case "DAMAGE":
                            if (typeSplit[2] != "99")
                                hactEvent = new CSVHActEventDamage();
                            break;
                        case "GAUGE":
                            hactEvent = new CSVHActEventHeatGauge();
                            break;
                    }

                    if (hactEvent == null)
                        hactEvent = new CSVHActEvent();

                    //Then read the data...
                    hactEvent.Type = type;
                    hactEvent.HEUnknown1 = reader.ReadInt32();

                    hactEvent.HEUnknown2 = reader.ReadInt32();
                    hactEvent.HEUnknown3 = reader.ReadInt32();
                    hactEvent.HEUnknown4 = reader.ReadInt32();
                    hactEvent.HEUnknown5 = reader.ReadInt32();
                    hactEvent.HEUnknown6 = reader.ReadInt32();
                    hactEvent.HEUnknown7 = reader.ReadInt32();

                    for (int l = 0; l < 4; l++)
                        hactEvent.Resources[l] = ReadStringPointer(reader.ReadInt32());

                    hactEvent.UnknownResource = ReadStringPointer(reader.ReadInt32());

                    long dataStart = reader.Stream.Position;
                    long dataTarget = reader.Stream.Position + 28;

                    hactEvent.ReadData(reader);

                    long unreadBytes = dataTarget - reader.Stream.Position;

                    if (unreadBytes < 0)
                    {
                        //overread
                        Debug.WriteLine($"[HACT_CSV] Overread {hactEvent.ToString()} by {unreadBytes * -1}");
                        reader.Stream.Position = dataTarget;
                    }
                    else
                    {
                        //underread
                        hactEvent.UnreadData = reader.ReadBytes((int)unreadBytes);
                    }
                    
                    hactEntry.SpecialNodes.Add(hactEvent);
                }

                //Read HAct unknown data
                reader.Stream.Seek(hactSection4DataPointer, SeekMode.Start);

                for(int j = 0; j < hactSection4DataCount; j++)
                {
                    CSVSection4 sec = new CSVSection4();
                    sec.Read(reader);

                    hactEntry.Section4.Add(sec);
                }

                reader.Stream.Seek(entryEnd);

                csv.Entries.Add(hactEntry);
            }

            reader.Stream.Seek(entriesPointer, SeekMode.Start);
            for (int i = 0; i < entriesCount; i++)
            {
                CSVHAct hact = csv.Entries[i];

                if (hact.Characters.Count == 0)
                    continue;

                //Last
                if (i == entriesCount - 1)
                {

                }
                else
                {
                    CSVHAct nextHAct = null;
                    int start = i + 1;
                    while(true)
                    {

                        if (csv.Entries[start].Characters.Count > 0)
                        {
                            nextHAct = csv.Entries[start];
                            break;
                        }

                        //super edge case i dont care about
                        if (start == entriesCount - 1)
                            Debug.Fail("Uh oh, this should never happen, tell Jhrino!");

                        start++;
                    }


                    int bytesToRead = m_extraDatPtr[nextHAct.Characters[0]] - m_extraDatPtr[hact.Characters[0]];
                    int numExtraDatas = bytesToRead / 48;
                    reader.Stream.Seek(m_extraDatPtr[hact.Characters[0]]);

                    for (int k = 0; k < numExtraDatas; k++)
                    {
                        CSVCharacterExtraData dat = new CSVCharacterExtraData();
                        dat.Read(reader);

                        hact.Characters[0].UnknownExtraData.Add(dat);
                    }
                }
            }

            return csv;
        }

        string ReadStringPointer(int addr)
        {
            if (addr <= 0)
                return null;
            else
            {
                string str = null;
                reader.Stream.RunInPosition(delegate { str = reader.ReadString(); }, addr, SeekMode.Start);

                return str;
            }
        }
    }
}
