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
            AuthPage page = reader.Read<AuthPage>();
            page.IsOldDE = CMN.LastGameVersion == GameVersion.DE1;

            if (!page.IsOldDE)
             //   page.PageTitleText = reader.ReadString(32);
                  page.PageTitleText = reader.ReadString(32).Split(new[] { '\0' }, 2)[0]; 

            //Read transitions
            // long transitionStart = startPos + 80 + (page.SkipLinkIndexNum * 4);

            page.SkipLink = new int[page.SkipLinkIndexNum];

            for (int i = 0; i < page.SkipLinkIndexNum; i++)
                page.SkipLink[i] = reader.ReadInt32();

            // reader.Stream.Seek(transitionStart, SeekMode.Start);

            page.Transitions = new List<Transition>(new Transition[page.TransitionCount]);

            for (int i = 0; i < page.TransitionCount; i++)
                page.Transitions[i] = (ProcessTransitionData(page));

            //Console.WriteLine(reader.ReadUInt32());
            //reader.Stream.Position -= 4;


            //DE 1.0: Fake talk page
            if(page.IsOldDE)
            {
                uint count = reader.ReadUInt32();
                reader.Stream.Position -= 4;

                if (count <= 0)
                    page.Flag = 0;
            }

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
        public uint Version { get; set; }
        public uint Flag { get; set; }

        public GameTick Start { get; set; } = new GameTick(0);
        public GameTick End { get; set; } = new GameTick(0);

        public int TransitionCount { get; set; }
        public int TransitionSize { get; set; }

        public List<Transition> Transitions = new List<Transition>();

        public int[] SkipLink = new int[0];
        public GameTick SkipTick { get; set; } = new GameTick(0);

        public int PageIndex { get; set; }
        public int SkipLinkIndexNum { get; set; }

        [BinaryString(FixedSize = 12, MaxSize = 12)]
        public string Padding { get; set; } = "";

        // [BinaryString(FixedSize = 32, MaxSize = 32)]
        public string PageTitleText = "";

        public bool IsOldDE = false;

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

        public AuthPage Clone()
        {
            return (AuthPage)MemberwiseClone();
        }

        public bool IsTalkPage()
        {
            if (IsOldDE)
                return Flag > 0;
            else
                return (Flag & 0x40) > 0;
        }


        //Get the size of page. Used for writing.
        public int GetPageSize()
        {
            int baseSize = (!IsOldDE ? 80 : 48) + GetTransitionSize() + (SkipLinkIndexNum * 4);

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
            return (uint)((!IsOldDE ? 80 : 48) + TransitionSize + (SkipLinkIndexNum * 4)); 
        }

        public uint Size()
        {
            if (IsTalkPage())
                return BaseSize() + 16 + TalkInfoHeader.Size;
            else
                return BaseSize();
        }
    }
}
