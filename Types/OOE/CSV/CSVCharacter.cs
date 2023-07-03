using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CSVCharacter
    {
        public string Name;
        public string ModelOverride;

        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int UnknownFlags1;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int ExitHActMode;

        public List<CSVCharacterExtraData> UnknownExtraData = new List<CSVCharacterExtraData>();

        internal void Read(DataReader reader)
        {
            Name = reader.ReadStringPointer(reader.ReadInt32());
            ModelOverride = reader.ReadStringPointer(reader.ReadInt32());

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            UnknownFlags1 = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
            Unknown10 = reader.ReadInt32();
            Unknown11 = reader.ReadInt32();
            Unknown12 = reader.ReadInt32();
            Unknown13 = reader.ReadInt32();
            Unknown14 = reader.ReadInt32();
            ExitHActMode = reader.ReadInt32();

            int unkDatPointer = reader.ReadInt32();
            int unkDatCount = reader.ReadInt32();

            if(unkDatCount > 0 && unkDatPointer > 0)
            {
                reader.Stream.RunInPosition(delegate
                {
                    CSVCharacterExtraData dat = new CSVCharacterExtraData();
                    dat.Read(reader);
                    UnknownExtraData.Add(dat);

                }, unkDatPointer, SeekMode.Start);
                }
        }

        internal void Write(DataWriter writer)
        {
            //Name and model override written later
            writer.Write(0xDEADC0DE);
            writer.Write(0xDEADC0DE);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(UnknownFlags1);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(Unknown12);
            writer.Write(Unknown13);
            writer.Write(Unknown14);

            writer.Write(ExitHActMode);

            //Extra data written later
            writer.Write(0xDEADC0DE);
            writer.Write(0xDEADC0DE);
        }

        public override string ToString()
        {
            return Name;
        }
    }


    public class CSVCharacterExtraData
    {
        public int Type;
        public byte[] Data;

        internal void Read(DataReader reader)
        {
            Type = reader.ReadInt32();

            switch(Type)
            {
                default:
                    Data = reader.ReadBytes(44);
                    break;
                case 0:
                    Data = reader.ReadBytes(92);
                    break;
                case 7:
                    Data = reader.ReadBytes(92);
                    break;
            }
        } 
        
        internal void Write(DataWriter writer)
        {
            //This isnt raw bytes. It has pointers in it
            writer.Write(Type);
            writer.Write(Data);
        }
    }
}
