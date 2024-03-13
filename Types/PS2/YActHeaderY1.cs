using System;


namespace HActLib.YAct
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    internal class YActHeaderY1
    {
        public int FileSize { get; set; }
        public SizedPointer Effects { get; set; }
        public SizedPointer Chunk2 { get; set; }
        public SizedPointer Chunk3 { get; set; }
        public SizedPointer CameraAnimations { get; set; }
        public SizedPointer Chunk5 { get; set; }
        public SizedPointer CharacterAnimations { get; set; }
        public SizedPointer Chunk7 { get; set; }
    }
}
