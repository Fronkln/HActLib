using System;
using System.Windows.Forms;
using System.Globalization;
using HActLib;

namespace CMNEdit
{
    internal static class PageWindow
    {
        public static void Draw(Pager form, TreeViewItemPage page)
        {
            form.CreateHeader("Page Info");

            if(page.Page.Format > 1)
                form.CreateInput("Name", page.Page.PageTitleText, delegate (string newVal) { page.Page.PageTitleText = newVal; page.Update(); });
            
            if(page.Page.Format >= 1)
                form.CreateInput("Page Index", page.Page.PageIndex.ToString(), delegate (string newVal) { page.Page.PageIndex = int.Parse(newVal); page.Update(); }, NumberBox.NumberMode.UInt);

            form.CreateInput("Flag", page.Page.Flag.ToString(), delegate (string newVal) { page.Page.Flag = uint.Parse(newVal); page.Update(); }, NumberBox.NumberMode.UInt);

            form.CreateInput("Start", page.Page.Start.Frame.ToString(), delegate (string newVal) { page.Page.Start.Frame = Utils.InvariantParse(newVal); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", page.Page.End.Frame.ToString(), delegate (string newVal) { page.Page.End.Frame = Utils.InvariantParse(newVal); }, NumberBox.NumberMode.Float);

            form.CreateInput("Skip", page.Page.SkipTick.Frame.ToString(), delegate (string newVal) { page.Page.SkipTick.Frame = Utils.InvariantParse(newVal); }, NumberBox.NumberMode.Float);

            if(page.Page.Flag == 4544)
            {
                foreach (TalkInfo talk in page.Page.TalkInfo)
                    DrawTalk(form, talk);
            }
        }
        
        private static void DrawTalk(Pager form, TalkInfo talk)
        {
            form.CreateHeader("Talk");
            form.CreateInput("Start", new GameTick(talk.StartTick).Frame.ToString(), delegate (string val) { talk.StartTick = new GameTick(Utils.InvariantParse(val)).Tick; }, NumberBox.NumberMode.Float);
            form.CreateInput("End", new GameTick(talk.EndTick).Frame.ToString(), delegate (string val) { talk.EndTick = new GameTick(Utils.InvariantParse(val)).Tick; }, NumberBox.NumberMode.Float);
        }
    }
}
