using Yarhl.FileFormat;
using Yarhl.IO;
using Yarhl.IO.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace HActLib
{
    public class AuthPageConverter : IConverter<BinaryFormat, AuthPage>
    {
        #region Read
        public AuthPage Convert(BinaryFormat source)
        {
            DataReader reader = new DataReader(source.Stream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };
            long startPos = reader.Stream.Position;

            Condition ProcessConditionData()
            {
                Condition cond = (Condition)ConvertFormat.With<ConditionConvert>(new BinaryFormat(reader.Stream));
                return cond;
            }

            Transition ProcessTransitionData(AuthPage authPage)
            {
                Transition trans = reader.Read<Transition>();

                trans.Conditions = new List<Condition>(new Condition[trans.ConditionCount]);

                for (int i = 0; i < trans.Conditions.Count; i++)
                    trans.Conditions[i] = ProcessConditionData();

                return trans;
            }

            //read basic info
            AuthPage page = new AuthPage();
            GameVersion ver = CMN.LastGameVersion;

            if (ver <= GameVersion.Yakuza6)
                page.Format = 0;
            else if (ver == GameVersion.DE1)
                page.Format = 1;
            else if (ver >= GameVersion.DE2)
                page.Format = 2;

            if(page.Format > 0)
            {
                page.Version = reader.ReadUInt32();
                page.Flag = reader.ReadUInt32();
            }

            page.Start = new GameTick(reader.ReadUInt32());
            page.End = new GameTick(reader.ReadUInt32());

            if (page.Format < 1)
                page.Unk = reader.ReadInt32();

            page.TransitionCount = reader.ReadInt32();
            page.TransitionSize = reader.ReadInt32();

            page.SkipTick = new GameTick(reader.ReadUInt32());

            if (page.Format > 0)
                page.PageIndex = reader.ReadInt32();

            page.SkipLinkIndexNum = reader.ReadInt32();

            if (page.Format < 1)
                reader.Stream.Position += 4;
            else
                reader.Stream.Position += 12;

            if (page.Format > 1)
                  page.PageTitleText = reader.ReadString(32, System.Text.Encoding.GetEncoding(932)).Split(new[] { '\0' }, 2)[0];

            //Read transitions
            // long transitionStart = startPos + 80 + (page.SkipLinkIndexNum * 4);

            page.SkipLink = new int[page.SkipLinkIndexNum];

            for (int i = 0; i < page.SkipLinkIndexNum; i++)
                page.SkipLink[i] = reader.ReadInt32();

            // reader.Stream.Seek(transitionStart, SeekMode.Start);

            page.Transitions = new List<Transition>(new Transition[page.TransitionCount]);

            for (int i = 0; i < page.TransitionCount; i++)
                page.Transitions[i] = (ProcessTransitionData(page));

            //read talk page related info
            if (page.IsTalkPage())
            {
                page.TalkInfoHeader = reader.Read<TalkInfoHeader>();
                page.TalkInfo = new TalkInfo[page.TalkInfoHeader.Count];

                for (int i = 0; i < page.TalkInfo.Length; i++)
                    page.TalkInfo[i] = reader.Read<TalkInfo>();
            }

            return page;
        }
        #endregion
    }

    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class AuthPage : IFormat
    {
        public uint Version;
        public uint Flag;

        public GameTick Start = new GameTick(0);
        public GameTick End = new GameTick(0);

        public int Unk = 0; //Y6

        public int TransitionCount = 0;
        public int TransitionSize = 0;

        public List<Transition> Transitions = new List<Transition>();

        public int[] SkipLink = new int[0];
        public GameTick SkipTick = new GameTick(0);

        public int PageIndex = 0;
        public int SkipLinkIndexNum = 0;

        [BinaryString(FixedSize = 12, MaxSize = 12)]
        public string Padding { get; set; } = "";

        public string PageTitleText = "";

        public int Format = 0; //0 = Y6, 1 = DE 1.0, 2 = DE 2.0

        /// <summary>
        /// The talk information header of the page if it exists. Otherwise null.
        /// </summary>
        public TalkInfoHeader TalkInfoHeader;

        /// <summary>
        /// The talk information of the page if it exists. Otherwise empty.
        /// </summary>
        public TalkInfo[] TalkInfo;

        public AuthPage()
        {

        }

        public AuthPage(string name)
        {
            PageTitleText = name;
        }

        public AuthPage(string name, float start, float end)
        {
            PageTitleText = name;
            Start.Frame = start;
            End.Frame = end;
            SkipTick.Frame = End.Frame;
        }

        public AuthPage(string name, float start, float end, int format)
        {
            PageTitleText = name;
            Start.Frame = start;
            End.Frame = end;
            Format = format;
        }

        public AuthPage Clone()
        {
            return (AuthPage)MemberwiseClone();
        }

        public bool IsTalkPage()
        {
            if (Format <= 1)
                return false; //29.12.2024 i dont think they exist k_playspot_batting YK2 and LADIW
            //return Flag > 0;
            else
                return (Flag & 0x40) > 0;
        }


        //Get the size of page. Used for writing.
        public int GetPageSize()
        {
            int pageSize = 0;

            if (Format == 0)
                pageSize = 32;
            else if (Format == 1)
                pageSize = 48;
            else
                pageSize = 80;

            int baseSize = (pageSize) + GetTransitionSize() + (SkipLinkIndexNum * 4);

            if (TalkInfo != null && TalkInfo.Length > 0)
                baseSize += 16 + (TalkInfo.Length * 16);

            return baseSize; 
        }

        //Get the size of transitions. Used for writing.
        public int GetTransitionSize()
        {
            int size = 0;

            for(int i = 0; i < Transitions.Count; i++)
            {
                Transition trans = Transitions[i];
                size += 16;
                size += trans.GetConditionSize();
            }

            return size;
        }

        public uint BaseSize()
        {
            if(Format == 0)
                return (uint)((32) + TransitionSize + (SkipLinkIndexNum * 4));
            else if(Format == 1)
                return (uint)((48) + TransitionSize + (SkipLinkIndexNum * 4));
            else
                return (uint)((80) + TransitionSize + (SkipLinkIndexNum * 4));
        }

        public uint Size()
        {
            if (IsTalkPage())
                return BaseSize() + 16 + TalkInfoHeader.Size;
            else
                return BaseSize();
        }

        public static int GetFormatForGame(Game game)
        {
            if (game == Game.Y6)
                return 0;

            if (game < Game.YLAD)
                return 1;

            return 2;
        }
        public static int GetFormatForGameVer(GameVersion ver)
        {
            if (ver == GameVersion.Yakuza6 || ver == GameVersion.Yakuza6Demo)
                return 0;

            if (ver == GameVersion.DE1)
                return 1;

            if (ver == GameVersion.DE2)
                return 2;

            return 2;
        }
    }
}
