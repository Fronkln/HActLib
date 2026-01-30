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
    [ElementID(Game.LADPYIH, 0x8E)]
    [ElementID(Game.YK3, 0x8E)]
    public class DEElementSpeech : NodeElement
    {
        public uint SpeechVersion;
        public ushort SpeechCuesheet;
        public ushort SpeechID;

        public int DontAffectPan;
        public int DontAffectDecay;
        public int DontDisplaySubtitles;

        public int CurveCount; //we dont read these for now

        public float Scale;
        public float ScaleNeck;

        public int DontPlayVoice;
        public int IsDramaScanner;

        public int IsNotAddMotion;
        public int IsWaitButton;
        public int MessageDelayFrame;
        public int MessageAdvanceFrame;

        public float SubtitleOffsetY;

        public int SpeechFlag;

        public byte[] V12Data = new byte[12];

        public int BHVPartsBlendSetID;
        public int BHVPartsBlendGroupID;
        public int BHVPartsBlendActionID;
        public float BHVPartsBlendScale;
        public int BHVPartsBlendFadeIn;
        public int BHVPartsBlendFadeOut;
        public string GMTFileName;


        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            SpeechVersion = reader.ReadUInt32();
            SpeechID = reader.ReadUInt16();
            SpeechCuesheet = reader.ReadUInt16();

            DontAffectPan = reader.ReadInt32();
            DontAffectDecay = reader.ReadInt32();
            DontDisplaySubtitles = reader.ReadInt32();

            CurveCount = reader.ReadInt32();

            /*
            Scale = reader.ReadSingle();
            ScaleNeck = reader.ReadSingle();

            DontPlayVoice = reader.ReadInt32();
            IsDramaScanner = reader.ReadInt32();

            int offset = reader.ReadInt32();

            if (offset > 0)
                throw new Exception("Speech TODO: Offset greater than 0");

            IsNotAddMotion = reader.ReadInt32();
            IsWaitButton = reader.ReadInt32();
            MessageDelayFrame = reader.ReadInt32();

            if (SpeechVersion >= 10)
            {
                MessageAdvanceFrame = reader.ReadInt32();
                SubtitleOffsetY = reader.ReadSingle();

                SpeechFlag = reader.ReadInt32();
            }

            BHVPartsBlendSetID = reader.ReadInt32();
            BHVPartsBlendGroupID = reader.ReadInt32();
            BHVPartsBlendActionID = reader.ReadInt32();
            BHVPartsBlendScale = reader.ReadSingle();
            BHVPartsBlendFadeIn = reader.ReadInt32();
            BHVPartsBlendFadeOut = reader.ReadInt32();

             reader.ReadBytes(44);

            if (SpeechVersion >= 12)
                V12Data = reader.ReadBytes(12);

            GMTFileName = reader.ReadString(32);


            int curveSize = CurveCount * 4;
            */
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            writer.Write(SpeechVersion);
            writer.Write(SpeechID);
            writer.Write(SpeechCuesheet);

            writer.Write(DontAffectPan);
            writer.Write(DontAffectDecay);
            writer.Write(DontDisplaySubtitles);

            writer.Write(CurveCount);

            /*
            writer.Write(Scale);
            writer.Write(ScaleNeck);

            writer.Write(DontPlayVoice);
            writer.Write(IsDramaScanner);

            int offset = 0;

            writer.Write(offset);

            writer.Write(IsNotAddMotion);
            writer.Write(IsWaitButton);
            writer.Write(MessageDelayFrame);

            if (SpeechVersion >= 10)
            {
                writer.Write(MessageAdvanceFrame);
                writer.Write(SubtitleOffsetY);

                writer.Write(SpeechFlag);
            }

            writer.Write(BHVPartsBlendSetID);
            writer.Write(BHVPartsBlendGroupID);
            writer.Write(BHVPartsBlendActionID);
            writer.Write(BHVPartsBlendScale);
            writer.Write(BHVPartsBlendFadeIn);
            writer.Write(BHVPartsBlendFadeOut);

            if (SpeechVersion >= 12)
                writer.Write(V12Data);

            writer.Write(GMTFileName.ToLength(32));
            */
        }
    }
}
