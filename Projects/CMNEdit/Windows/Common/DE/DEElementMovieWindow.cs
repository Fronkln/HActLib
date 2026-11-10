using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementMovieWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementMovie mov = node as DEElementMovie;

            PropertyEditorBuilder.CreateHeader("Movie");
            PropertyEditorBuilder.CreateInput("Version", mov.MovieVersion.ToString(), delegate (string val) { mov.MovieVersion = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Movie", mov.Movie, delegate (string val) { mov.Movie = val; }, NumberBox.NumberMode.Text);
            PropertyEditorBuilder.CreateInput("Dont Fade", Convert.ToInt32(mov.DontFade).ToString(), delegate (string val) { mov.DontFade = byte.Parse(val) == 1; }, NumberBox.NumberMode.Byte);
            PropertyEditorBuilder.CreateInput("Fade Frame", mov.FadeFrame.ToString(), delegate (string val) { mov.FadeFrame = float.Parse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
