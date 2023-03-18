using System;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActEndWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActEnd end = node as OEHActEnd;

            form.CreateHeader("HAct End");

            form.CreateInput("Unknown", end.Unknown.ToString(), delegate (string val) { end.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
