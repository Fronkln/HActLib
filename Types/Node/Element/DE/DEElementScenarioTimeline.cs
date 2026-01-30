using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x18)]
    [ElementID(Game.YK2, 0x18)]
    [ElementID(Game.JE, 0x18)]
    [ElementID(Game.YLAD, 0x17)]
    [ElementID(Game.LJ, 0x17)]
    [ElementID(Game.LAD7Gaiden, 0x17)]
    [ElementID(Game.LADIW, 0x17)]
    [ElementID(Game.LADPYIH, 0x17)]
    [ElementID(Game.YK3, 0x17)]
    public class DEElementScenarioTimeline : NodeElement
    {
        public string TimelineCategory;
        public string Timeline;
        public string Clock;
        public bool OnlyFlagControl;
        public bool FlagState;
        public bool FinishOff;
        public uint UIDListNum;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            TimelineCategory = reader.ReadString(32).Split(new[] { '\0' }, 2)[0];
            Timeline = reader.ReadString(32).Split(new[] { '\0' }, 2)[0];
            Clock = reader.ReadString(64).Split(new[] { '\0' }, 2)[0];
            OnlyFlagControl = reader.ReadInt32() > 0;
            FlagState = reader.ReadInt32() > 0;
            FinishOff = reader.ReadInt32() > 0;
            UIDListNum = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(TimelineCategory.ToLength(32));
            writer.Write(Timeline.ToLength(32));
            writer.Write(Clock.ToLength(64));
            writer.Write(Convert.ToInt32(OnlyFlagControl));
            writer.Write(Convert.ToInt32(FlagState));
            writer.Write(Convert.ToInt32(FinishOff));
            writer.Write(UIDListNum);
        }
    }
}
