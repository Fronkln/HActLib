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
    [ElementID(Game.FOTNS, 96)]
    public class OESubtitle : NodeElement
    {
        public class SubtitleEntry
        {
            public float Start = 0;
            public float End = 0;

            public string Text = "";
        }

        public int UnkNum;
        public int UnkNum2;

        public List<SubtitleEntry> Subtitles = new List<SubtitleEntry>();
        public List<SubtitleEntry> SubtitlesJP = new List<SubtitleEntry>(); //v16

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            uint count = reader.ReadUInt32();
            UnkNum = reader.ReadInt32();
            UnkNum2 = reader.ReadInt32();

            if(inf.version <= 15)
                reader.Stream.Position += 4;
            else
            {
                int size = reader.ReadInt32(); // size * 8
                reader.Stream.Position += 8;
            }

            if(inf.version <= 15)
            {
                for (int i = 0; i < count; i++)
                {
                    SubtitleEntry entry = new SubtitleEntry();

                    entry.Start = reader.ReadSingle();
                    entry.End = reader.ReadSingle();

                    reader.ReadBytes(8);

                    entry.Text = reader.ReadString(128).Split(new[] { '\0' }, 2)[0];

                    Subtitles.Add(entry);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    SubtitleEntry entry = new SubtitleEntry();

                    entry.Start = reader.ReadSingle();
                    entry.End = reader.ReadSingle();

                    reader.ReadBytes(8);

                    entry.Text = reader.ReadString(256, Encoding.UTF8).Split(new[] { '\0' }, 2)[0];

                    SubtitlesJP.Add(entry);
                }

                for (int i = 0; i < count; i++)
                {
                    SubtitleEntry entry = new SubtitleEntry();

                    entry.Start = reader.ReadSingle();
                    entry.End = reader.ReadSingle();

                    reader.ReadBytes(8);

                    entry.Text = reader.ReadString(256).Split(new[] { '\0' }, 2)[0];

                    Subtitles.Add(entry);
                }
            }
        }

        public override void OE_ConvertToY0()
        {
            base.OE_ConvertToY0();

            SubtitlesJP = Subtitles;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            long start = writer.Stream.Position;

            writer.Write(Subtitles.Count);
            writer.Write(UnkNum);
            writer.Write(UnkNum2);

            long sizeStart = 0;

            if (hactVer <= 15)
                writer.WriteTimes(0, 4);
            else
            {
                sizeStart = writer.Stream.Position;
                writer.Write(0xDEADC0DE); //size
                writer.WriteTimes(0, 8);
            }

            if (hactVer <= 15)
            {
                foreach (SubtitleEntry entry in Subtitles)
                {
                    writer.Write(entry.Start);
                    writer.Write(entry.End);

                    writer.WriteTimes(0, 8);

                    writer.Write(entry.Text.ToLength(128), 128, false, Encoding.GetEncoding(932));
                }
            }
            else
            {
                foreach(var entry in SubtitlesJP)
                {
                    writer.Write(entry.Start);
                    writer.Write(entry.End);

                    writer.WriteTimes(0, 8);

                    writer.Write(entry.Text.ToLength(256), 256, false, Encoding.UTF8);
                }

                foreach (var entry in Subtitles)
                {
                    writer.Write(entry.Start);
                    writer.Write(entry.End);

                    writer.WriteTimes(0, 8);

                    writer.Write(entry.Text.ToLength(256), 256, false, Encoding.UTF8);
                }


                int size = (int)(writer.Stream.Position - start) + 24;

                writer.Stream.RunInPosition(delegate
                {
                    writer.Write(size / 8);
                }, sizeStart, SeekMode.Start);
            }
        }
    }
}
