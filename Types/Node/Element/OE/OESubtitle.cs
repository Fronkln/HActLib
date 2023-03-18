using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 97)]
    [ElementID(Game.Ishin, 97)]
    [ElementID(Game.Y0, 95)]
    [ElementID(Game.YK1, 95)]
    public class OESubtitle : NodeElement
    {
        public class SubtitleEntry
        {
            public float Start = 0;
            public float End = 0;

            public string Text = "";
        }

        public int UnkNum;

        public List<SubtitleEntry> Subtitles = new List<SubtitleEntry>();

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint count = reader.ReadUInt32();
            UnkNum = reader.ReadInt32();

            reader.ReadBytes(8);


            for(int i = 0; i < count; i++)
            {
                SubtitleEntry entry = new SubtitleEntry();
                
                entry.Start = reader.ReadSingle();
                entry.End = reader.ReadSingle();
                
                reader.ReadBytes(8);
                
                entry.Text = reader.ReadString(128);

                Subtitles.Add(entry);
            }
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Subtitles.Count);
            writer.Write(UnkNum);

            writer.WriteTimes(0, 8);

            foreach(SubtitleEntry entry in Subtitles)
            {
                writer.Write(entry.Start);
                writer.Write(entry.End);

                writer.WriteTimes(0, 8);

                writer.Write(entry.Text.ToLength(128), 128, false, Encoding.GetEncoding(932));
            }
        }
    }
}
