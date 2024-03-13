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

        private static Dictionary<int, ObjectBase> m_objectPtrs = new Dictionary<int, ObjectBase>();

        public TEV Convert(BinaryFormat format)
        {
            m_dataPtr1End = 0;
            m_objectPtrs.Clear();

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

            tev.TEVHeader.CameraCount3 = reader.ReadInt32();
            tev.TEVHeader.UnkPtr2 = reader.ReadUInt32();

            tev.TEVHeader.UseSoundACB = reader.ReadUInt32() > 0;
            tev.TEVHeader.UnkPtr3 = reader.ReadUInt32();

            tev.TEVHeader.CameraCount = reader.ReadInt32();
            tev.TEVHeader.CameraCount2 = reader.ReadInt32();
            tev.TEVHeader.CharacterCount = reader.ReadInt32();
            tev.TEVHeader.CharacterCount2 = reader.ReadInt32();

            tev.TEVHeader.UnkRegion1 = reader.ReadBytes(32);
            tev.TEVHeader.UnkRegion2 = reader.ReadBytes(28);

            reader.Stream.RunInPosition(
                delegate 
                {
                    reader.Endianness = EndiannessMode.LittleEndian;
                    tev.CuesheetID = reader.ReadUInt32();
                    reader.Endianness = EndiannessMode.BigEndian;
                }, tev.TEVHeader.UnkPtr2, SeekMode.Start);

            reader.Stream.Seek(set1Start);
            ReadSet1(tev, reader, set1Count);
       
            reader.Stream.Seek(set2Start);
           // int set3Count = ReadSet2(tev, reader, set2Count);

            reader.Stream.Seek(set3Ptr);

            //foreach (ObjectBase set in tev.Objects)
               // set.OnLoadComplete(tev);

            tev.TEVHeader.StringTableOffset = (int)(tev.TEVHeader.DataPtr2 - tev.TEVHeader.UnkPtr3);
            tev.TEVHeader.DataPadding = (int)(tev.TEVHeader.DataPtr2 - m_dataPtr1End);

            return tev;
        }

        private static void ReadSet1(TEV tev, DataReader reader, uint set1Count)
        {

            for (int i = 0; i < set1Count; i++)
            {
                int pos = (int)reader.Stream.Position;
                ObjectBase tevObject = ObjectBase.ReadFromMemory(reader);

                m_objectPtrs.Add(pos, tevObject);

                //Lol
                if (i == 0)
                    tev.Root = tevObject;
                else if (tevObject._InternalInfo.Parent > -1)
                {
                    m_objectPtrs[tevObject._InternalInfo.Parent].Children.Add(tevObject);
                    tevObject.Parent = m_objectPtrs[tevObject._InternalInfo.Parent];
                }
                
            }
        }
    }
}
