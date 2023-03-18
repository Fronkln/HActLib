using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;
using System.Threading;
using System.Diagnostics;
using System.Runtime;
using System.IO;

namespace HActLib
{
    public class TEVReader : IConverter<BinaryFormat, TEV>
    {
        private static long m_dataPtr1End;

        public TEV Convert(BinaryFormat format)
        {
            DataReader reader = new DataReader(format.Stream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };
            TEV tev = new TEV();

            tev.TEVHeader.Magic = reader.ReadString(4);

            if (tev.TEVHeader.Magic != "TCAH")
                throw new Exception("Not HAct TEV");

            tev.TEVHeader.Version = reader.ReadUInt32();
            tev.TEVHeader.FormatDate = reader.ReadUInt32();
            tev.TEVHeader.Unk1 = reader.ReadUInt32();

            uint set1Start = reader.ReadUInt32();
            uint set1Count = reader.ReadUInt32();

            uint set2Start = reader.ReadUInt32();
            uint set2Count = reader.ReadUInt32();

            uint set3Ptr = reader.ReadUInt32();
            uint dataPtr1 = reader.ReadUInt32();

            m_dataPtr1End = dataPtr1;

            tev.TEVHeader.UnkCount1 = reader.ReadUInt32();
            tev.TEVHeader.DataPtr2 = reader.ReadUInt32();
            ;

            tev.TEVHeader.UnkCount2 = reader.ReadUInt32();
            tev.TEVHeader.UnkPtr2 = reader.ReadUInt32();

            tev.TEVHeader.UnkCount3 = reader.ReadUInt32();
            tev.TEVHeader.UnkPtr3 = reader.ReadUInt32();

            tev.TEVHeader.UnkVal1 = reader.ReadUInt32();
            tev.TEVHeader.UnkVal2 = reader.ReadUInt32();
            tev.TEVHeader.UnkVal3 = reader.ReadUInt32();
            tev.TEVHeader.UnkVal4 = reader.ReadUInt32();

            tev.TEVHeader.UnkRegion1 = reader.ReadBytes(32);
            tev.TEVHeader.UnkRegion2 = reader.ReadBytes(28);

            reader.Stream.RunInPosition(
                delegate 
                {
                    reader.Endianness = EndiannessMode.LittleEndian;
                    tev.CuesheetID = reader.ReadUInt32();
                    reader.Endianness = EndiannessMode.BigEndian;
                }, tev.TEVHeader.UnkPtr2, SeekMode.Start);

            tev.Objects = new ObjectBase[set1Count];
            tev.Set2 = new Set2[set2Count];

            reader.Stream.Seek(set1Start);
            ReadSet1(tev, reader, set1Count);
       
            reader.Stream.Seek(set2Start);
            int set3Count = ReadSet2(tev, reader, set2Count);

            reader.Stream.Seek(set3Ptr);
            ReadSet3(tev, reader, set3Count);

            foreach (ObjectBase set in tev.Objects)
                set.OnLoadComplete(tev);

            tev.TEVHeader.StringTableOffset = (int)(tev.TEVHeader.DataPtr2 - tev.TEVHeader.UnkPtr3);
            tev.TEVHeader.DataPadding = (int)(tev.TEVHeader.DataPtr2 - m_dataPtr1End);

            return tev;
        }

        private static void ReadSet1(TEV tev, DataReader reader, uint set1Count)
        {
            for (int i = 0; i < set1Count; i++)
            {
                uint type = 0;
                uint category = 0;
                reader.Stream.RunInPosition
                    (
                        delegate
                        {
                            type = reader.ReadUInt32();
                            category = reader.ReadUInt32();
                        }, 20, SeekMode.Current
                    );

                ObjectBase set = GetSet1Type(type, category);
                tev.PointerSet1[(int)reader.Stream.Position] = set;
                ReadBasicSet1Info(reader, tev, set);
            }
        }

        private static void ReadBasicSet1Info(DataReader reader, TEV tev, ObjectBase set)
        {
            string[] ReadStringTable(int[] table)
            {
                long curPos = reader.Stream.Position;

                List<string> stringTbl = new List<string>();

                foreach (int i in table)
                {
                    if (i != -1)
                    {
                        reader.Stream.Seek(i);
                        stringTbl.Add(reader.ReadString());
                    }
                    else
                        stringTbl.Add(null);
                }

                reader.Stream.Seek(curPos, SeekMode.Start);

                return stringTbl.ToArray();
            }

            set._InternalInfo.Addr = (uint)reader.Stream.Position;
            tev.Objects[reader.ReadUInt32()] = set;

            int[] stringTable = new int[]
            {
                reader.ReadInt32(),
                reader.ReadInt32(),
                reader.ReadInt32(),
                reader.ReadInt32()
            };

            set._InternalInfo.StringTables = stringTable;

            set.StringTable = ReadStringTable(stringTable);
            set.Type = (ObjectNodeCategory)reader.ReadUInt32();
            set.Category = reader.ReadUInt32();
            set.Unk1 = reader.ReadBytes(296);
            set.UnkFloatArray = new float[]
            {
               reader.ReadSingle(),
               reader.ReadSingle(),
               reader.ReadSingle(),
               reader.ReadSingle(),
            };

            set._InternalInfo.Parent = reader.ReadInt32();
            set._InternalInfo.PreviousNode = reader.ReadInt32();
            set._InternalInfo.NextNode = reader.ReadInt32();
            set._InternalInfo.NextMainNode = reader.ReadInt32();


            set._InternalInfo.Set2Ptr = reader.ReadInt32();
            set._InternalInfo.UnkNum1 = reader.ReadUInt32();

            set._InternalInfo.Set3Ptr = reader.ReadInt32();
            set._InternalInfo.DataPtr1 = reader.ReadInt32();
            set._InternalInfo.DataPtr2 = reader.ReadInt32();

            reader.ReadBytes(8);

            long curReadPos = reader.Stream.Position;

            set.ProcessNodeData(reader);

            if (set._InternalInfo.DataPtr1 > -1)
                m_dataPtr1End += 32;


            reader.Stream.Seek(curReadPos);
        }

        //Returns the count of set 3 elements.
        private static int ReadSet2(TEV tev, DataReader reader, uint set2Count)
        {
            //The amount of set 3 nodes are calculated here.
            int set3Count = 0;

            for (int i = 0; i < set2Count; i++)
            {
                uint type = 0;
                uint elementID = 0; //applicable if type == 9
                reader.Stream.RunInPosition
                    (
                        delegate
                        {
                            type = reader.ReadUInt32();
                            reader.ReadBytes(4);
                            elementID = reader.ReadUInt32();
                        }, 8, SeekMode.Current
                    );

                if (type == 9 && elementID != 1019)
                    set3Count++;

                Set2 set = GetSet2Type(type, elementID);
                set._InternalInfo.Addr = (uint)reader.Stream.Position;

                tev.PointerSet2.Add((int)reader.Stream.Position, set);

                ReadBasicSet2Info(reader, tev, set);
            }

            return set3Count;
        }

        private static void ReadSet3(TEV tev, DataReader reader, int set3Count)
        {
            tev.Effects = new EffectBase[set3Count];

            for(int i = 0; i < set3Count; i++)
            {
                int effectAddr = (int)reader.Stream.Position;
                EffectBase effect = EffectBase.ReadFromMemory(reader);
                
                tev.PointerSet3.Add(effectAddr, effect);
                tev.Effects[i] =effect;
            }
        }

        private static EffectBase CreateEffectObject(EffectID id)
        {
            switch(id)
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

        private static void ReadBasicSet2Info(DataReader reader, TEV tev, Set2 set)
        {
            long startPos = reader.Stream.Position;
            long expectedPos = startPos + 336;

            tev.Set2[reader.ReadUInt32()] = set;
            set._InternalInfo.resourcePtr = reader.ReadInt32();

            set.Type = (Set2NodeCategory)reader.ReadInt32();
            set.Unk1 = reader.ReadInt32();
            set._InternalInfo.elementType = reader.ReadInt32();

            set.ReadArgs(reader);
            set.Start = reader.ReadSingle();
            set.End = reader.ReadSingle();
            set.Fps = reader.ReadSingle();
            set.Unk3 = reader.ReadInt32();

            for (int i = 0; i < 8; i++)
                set.UnkSpeed[i] = reader.ReadSingle();

            long curReadPos = reader.Stream.Position;

            set.ProcessNodeData(reader);         
            reader.Stream.Seek(curReadPos);

            set.Unk4 = reader.ReadBytes(16);

            if (reader.Stream.Position != expectedPos)
                throw new Exception($"TEV Read Error: Set2 size mismatch. Expected to read 336 bytes, got: {reader.Stream.Position - startPos}");
        }


        private static ObjectBase GetSet1Type(uint type, uint category)
        {
            switch(type)
            {
                default:
                    return new ObjectBase();

                case 1:
                    return new ObjectCamera();
                case 2:
                    return new ObjectPath();
                case 3:
                    if (category >= 0 && category <= 1)
                        return new ObjectHuman();
                    else if (category == 2)
                        return new ObjectWeapon();
                    goto default;
                case 4:
                    return new ObjectModel();

                case 5:
                    return new ObjectBone();
                case 6:
                    return new ObjectItem();
            }
 
        }

        private static Set2 GetSet2Type(uint type, uint elementType)
        {
            switch (type)
            {
                default:
                    return new Set2();
                    
                case 1:
                    return new Set2ElementMotion();
                case 2:
                    return new Set2ElementMotion();
                case 3:
                    return new Set2ElementMotion();
                case 9:
                    switch(elementType)
                    {
                        default:
                            return new Set2Element();
                        case 1002:
                            return new Set2ElementParticle();
                        case 1019:
                           return new Set2Element1019();
                    }
                    
            }

        }
    }
}
