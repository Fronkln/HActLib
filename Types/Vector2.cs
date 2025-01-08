using Yarhl.IO;

namespace HActLib
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Write(DataWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
        }
    }
}
