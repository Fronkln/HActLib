using HActLib;
using System;

namespace CMNEdit
{
    internal static class DEElementTalkTextWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementTalkText text = node as DEElementTalkText;

            form.CreateHeader("Talk Text");
            form.CreateInput("Category ID", text.TalkCategory.ToString(), delegate (string var) { text.TalkCategory = uint.Parse(var); }, NumberBox.NumberMode.Int);
            form.CreateInput("Text ID", text.TextID.ToString(), delegate (string var) { text.TextID = uint.Parse(var); }, NumberBox.NumberMode.Int);
        }
    }
}
