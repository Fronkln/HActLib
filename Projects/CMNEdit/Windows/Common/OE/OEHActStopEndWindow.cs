using System;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActStopEndWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActStopEnd stopEnd = node as OEHActStopEnd;

            form.CreateHeader("HAct Stop End");
            form.CreateInput("Unknown", stopEnd.Unknown.ToString(), delegate (string val) { stopEnd.Unknown = short.Parse(val); }, NumberBox.NumberMode.Ushort);
        }
    }
}
