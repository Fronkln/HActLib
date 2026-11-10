using HActLib;
using System;

namespace CMNEdit
{
    internal static class DEElementTalkTextWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementTalkText text = node as DEElementTalkText;

            PropertyEditorBuilder.CreateHeader("Talk Text");
            PropertyEditorBuilder.CreateInput("Category ID", text.TalkCategory.ToString(), delegate (string var) { text.TalkCategory = uint.Parse(var); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Text ID", text.TextID.ToString(), delegate (string var) { text.TextID = uint.Parse(var); }, NumberBox.NumberMode.Int);
        }
    }
}
