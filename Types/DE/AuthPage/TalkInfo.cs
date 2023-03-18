using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    [Serializable]
    public class TalkInfo
    {
        public uint StartTick { get; set; }
        public uint EndTick { get; set; }
        public uint Flag { get; set; }

        [BinaryString(FixedSize = 4, MaxSize = 4)]
        public string Padding { get; set; }
    }
}
