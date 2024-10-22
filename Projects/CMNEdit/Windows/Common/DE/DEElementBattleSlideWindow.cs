using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementBattleSlideWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementBattleSlide slide = node as DEElementBattleSlide;

            form.CreateHeader("Battle Slide");
            form.CreateInput("Slide Type", slide.SlideType.ToString(), delegate (string val) { slide.SlideType = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
