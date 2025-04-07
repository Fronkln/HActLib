using System;

namespace HActLib
{
    public class AuthNodeOOEAnimationData
    {
        public int Unknown1;
        public int Unknown2;
        public Guid Guid;

        public float StartFrame;
        public float EndFrame;

        public byte[] UnknownData = new byte[96];
    }
}
