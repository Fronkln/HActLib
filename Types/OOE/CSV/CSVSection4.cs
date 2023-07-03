using Yarhl.IO;

namespace HActLib
{
    public class CSVSection4
    {
        public int Unknown;
        public string[] Resources = new string[3];
        public byte[] Unknown2;

        internal void Read(DataReader reader)
        {
            Unknown = reader.ReadInt32();

            int[] resourcePtrs = new int[Resources.Length];

            for (int i = 0; i < Resources.Length; i++)
                resourcePtrs[i] = reader.ReadInt32();

            Unknown2 = reader.ReadBytes(48);

            for (int i = 0; i < Resources.Length; i++)
                Resources[i] = reader.ReadStringPointer(resourcePtrs[i]);
        }

        internal void Write(DataWriter writer)
        {
            writer.Write(Unknown);

            for (int i = 0; i < Resources.Length; i++)
                writer.Write(0xDEADC0DE);

            writer.Write(Unknown2);
        }
    }
}
