using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    //Introduced in Pirate game
    [Serializable]
    public class GameTick2
    {
        public uint Tick { get; set; }
        [BinaryIgnore]
        public float Frame
        {
            get { return TickToFrame(Tick); }
            set { Tick = FrameToTick(value); }
        }

        ///<summary>Convert Dragon Engine tick to frame.</summary>
        public static float TickToFrame(uint tick)
        {
            return tick / 25600.0f;
        }

        ///<summary>Convert frame to Dragon Engine tick </summary>
        public static uint FrameToTick(float frame)
        {
            return (uint)(frame * 25600.0f);
        }

        public GameTick2()
        {

        }
        public GameTick2(uint tick)
        {
            Tick = tick;
        }

        public GameTick2(float frame)
        {
            Frame = frame;
        }

        public static implicit operator uint(GameTick2 tick)
        {
            return tick.Tick;
        }

        public static implicit operator float(GameTick2 tick)
        {
            return tick.Frame;
        }

        public static implicit operator GameTick(GameTick2 tick)
        {
            return new GameTick(tick.Frame);
        }
    }
}
