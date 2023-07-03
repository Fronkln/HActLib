using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 0x92)]
    [ElementID(Game.YK2, 0x92)]
    [ElementID(Game.JE, 0x92)]
    [ElementID(Game.YLAD, 0x8E)]
    [ElementID(Game.LJ, 0x8E)]
    [ElementID(Game.LAD7Gaiden, 0x8E)]
    [ElementID(Game.LADIW, 0x8E)]
    public class DEElementSpeech : NodeElement
    {
        public uint SpeechVersion;
        public ushort SpeechCuesheet;
        public ushort SpeechID;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            SpeechVersion = reader.ReadUInt32();
            SpeechID = reader.ReadUInt16();
            SpeechCuesheet = reader.ReadUInt16();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(SpeechVersion);
            writer.Write(SpeechID);
            writer.Write(SpeechCuesheet);
        }
    }
}
