using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    [Serializable]
    public class DisableFrameInfo
    {
        public float Start { get; set; }
        public float End { get; set; }
    }
}
