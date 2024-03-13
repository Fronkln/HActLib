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
        public string Name = "CHARACTER";
        public string ModelOverride = "";

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
        public int ExitHActMode = 0;

        public int UnknownNum;
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
        public int Unknown = 7;
        public string[] Resources = new string[3] { "", "", "" };
        public int Unknown2 = 0;
        public int Unknown3 = 0;
        public int Unknown4 = 1077936128;
        public int Unknown5 = 0;
        public int Unknown6 = 0;
        public int Unknown7 = 0;
        public int Unknown8 = 0;
        public int Unknown9 = 0;

        
        internal void Read(DataReader reader)
        {
            Unknown = reader.ReadInt32();

            Resources = new string[3];

            for (int i = 0; i < Resources.Length; i++)
                Resources[i] = reader.ReadStringPointer(reader.ReadInt32());

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
        }

        internal void Write(DataWriter writer)
        {
            writer.Write(Unknown);

            foreach (string str in Resources)
                writer.Write(0xDEADC0DE);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
        }
    }

    public enum HumanReturnMode
    {
        Stand,
        Down,
        Unknown1,
        Unknown2,
        Unknown3,
        Unknown4,
        Unknown5,
        Unknown6,
        Unknown7,
    }
}
