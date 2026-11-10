using HActLib;
using System;
using System.Collections.Generic;
using System.Text;
using YakuzaDataTypes.MSG;

namespace CMNEdit
{
    internal static class FileContext
    {
        public static string FilePath;

        public static bool IsBep;
        public static bool IsMep;
        public static bool IsHact;
        public static bool IsTev;
        public static bool IsOE;
        public static bool IsYAct;
        public static bool IsPS2Prop;
        public static bool IsOOEAuth;
        public static bool IsMsg;

        public static MEP Mep;

        public static TEV Tev;
        public static CSV Csv;
        public static Auth Auth;
        public static uint TevHActID;
        public static CSVHAct TevCsvEntry;

        public static Msg Msg;
        public static MsgOOE MsgOOE;

        public static GameVersion curVer;
        public static Game curGame = Game.YLAD; //only used for DE

        public static void Reset()
        {
            IsBep = false;
            IsMep = false;
            IsHact = false;
            IsTev = false;
            IsOE = false;
            IsPS2Prop = false;
            IsYAct = false;
            IsMsg = false;
        }

    }
}
