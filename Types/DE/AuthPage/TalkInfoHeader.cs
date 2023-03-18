using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    [Serializable]
    public class TalkInfoHeader
    {
        public uint Count { get; set; }
        public uint Size { get; set; }
        public uint Flags { get; set; }

        [BinaryString(FixedSize = 4, MaxSize = 4)]
        public string Padding { get; set; }
    }
}
