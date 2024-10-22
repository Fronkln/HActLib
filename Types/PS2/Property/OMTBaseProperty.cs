using Yarhl.IO;

namespace HActLib
{
    public class OMTBaseProperty
    {
        public int Type { get; internal set; }
        public float Start;
        public float End;
        public float UnknownFloat;

        public byte[] UnknownData;

        internal void Read(DataReader reader)
        {
            //type would have been read already
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            UnknownFloat = reader.ReadSingle();
            ReadData(reader);
        }

        internal virtual void ReadData(DataReader reader)
        {

        }

        internal void Write(DataWriter writer)
        {
            writer.Write(Type);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(UnknownFloat);
            WriteData(writer);

            if (UnknownData != null)
                writer.Write(UnknownData);
        }

        internal virtual void WriteData(DataWriter writer)
        {

        }

        internal virtual int GetPropertyType()
        {
            return Type;
        }
    }
}
