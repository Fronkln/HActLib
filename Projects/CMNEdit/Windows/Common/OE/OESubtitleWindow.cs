using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OESubtitleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OESubtitle subtitle = node as OESubtitle;

            if (subtitle == null)
                return;

            PropertyEditorBuilder.CreateHeader("Subtitle");

            PropertyEditorBuilder.CreateSpace(15);

            for (int i = 0; i < subtitle.Subtitles.Count; i++)
            {
                OESubtitle.SubtitleEntry entry = subtitle.Subtitles[i];
                DrawEntry(form, entry, i);
            }

            if (subtitle.SubtitlesJP.Count > 0)
            {
                PropertyEditorBuilder.CreateHeader("Subtitles(JP)");

                for (int i = 0; i < subtitle.SubtitlesJP.Count; i++)
                {
                    OESubtitle.SubtitleEntry entry = subtitle.SubtitlesJP[i];
                    DrawEntry(form, entry, i);
                }
            }

            PropertyEditorBuilder.CreateButton("Add New", delegate { subtitle.Subtitles.Add(new OESubtitle.SubtitleEntry()); subtitle.SubtitlesJP.Add(new OESubtitle.SubtitleEntry()); Form1.Instance.ProcessSelectedNode(Form1.EditingNode); });
            PropertyEditorBuilder.CreateButton("Insert New", delegate
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Insert subtitle index to insert to", "New Subtitle", "");

                if (string.IsNullOrEmpty(input))
                    return;

                int idx = 0;

                if (!int.TryParse(input, out idx))
                    return;

                if (idx < 0 || idx >= subtitle.Subtitles.Count)
                    return;

                subtitle.Subtitles.Insert(idx, new OESubtitle.SubtitleEntry());
                subtitle.SubtitlesJP.Insert(idx, new OESubtitle.SubtitleEntry());
                Form1.Instance.ProcessSelectedNode(Form1.EditingNode);
            });
            PropertyEditorBuilder.CreateButton("Clear All", delegate { subtitle.Subtitles.Clear(); subtitle.SubtitlesJP.Clear(); Form1.Instance.ProcessSelectedNode(Form1.EditingNode); });
        }

        private static void DrawEntry(Form1 form, OESubtitle.SubtitleEntry entry, int idx)
        {
            PropertyEditorBuilder.CreateInput($"Subtitle {idx} Start", entry.Start.ToString(), delegate (string val) { entry.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput($"Subtitle {idx} End", entry.End.ToString(), delegate (string val) { entry.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateMultilineInput($"Subtitle {idx} Text", entry.Text.ToString(), delegate (string val) { entry.Text = val; }, 50);

            PropertyEditorBuilder.CreateSpace(25);
        }
    }
}
