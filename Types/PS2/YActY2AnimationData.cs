using HActLib.PS2;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActY2AnimationData
    {
        public float Unknown1;
        public float Start;
        public float End;
        public float Unknown2;
        public int AnimationID;
        public byte[] UnknownDat1 = new byte[12];

        //Value doesnt matter, its only for info
        public OGREAnimationFormat Format { get; internal set; }

        /// <summary>
        /// The buffer of the file referenced through animation ID
        /// </summary>
        public YActFile File = new YActFile();

        internal void Read(DataReader reader)
        {
            Unknown1 = reader.ReadSingle();
            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Unknown2 = reader.ReadSingle();
            AnimationID = reader.ReadInt32();
            UnknownDat1 = reader.ReadBytes(12);
        }

        internal void Write(DataWriter writer)
        {
            writer.Write(Unknown1);
            writer.Write(Start);
            writer.Write(End);
            writer.Write(Unknown2);
            writer.Write(AnimationID);
            writer.Write(UnknownDat1);
        }
      
    }
}
