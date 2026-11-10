using System;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActEndWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActEnd end = node as OEHActEnd;

            PropertyEditorBuilder.CreateHeader("HAct End");

            PropertyEditorBuilder.CreateInput("Unknown", end.Unknown.ToString(), delegate (string val) { end.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
