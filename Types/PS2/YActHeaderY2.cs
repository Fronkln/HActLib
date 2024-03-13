using System;


namespace HActLib.YAct
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    internal class YActHeaderY2
    {
        public SizedPointer Chunk1 { get; set; }
        public SizedPointer Character { get; set; }
        public SizedPointer Chunk3 { get; set; }
        public SizedPointer Chunk4 { get; set; }
        public SizedPointer Chunk5 { get; set; }
        public SizedPointer Effect { get; set; }
        public SizedPointer Chunk7 { get; set; }
        public SizedPointer Chunk8 { get; set; }
        public SizedPointer Camera { get; set; }
        public SizedPointer Chunk10 { get; set; }
        public SizedPointer CharacterAnimations { get; set; }
        public SizedPointer CameraAnimations { get; set; }
        public SizedPointer StringTable { get; set; }
        public SizedPointer Chunk13 { get; set; }
    }
}
