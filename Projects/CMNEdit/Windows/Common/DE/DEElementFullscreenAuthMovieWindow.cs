using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementFullscreenAuthMovieWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementFullscreenAuthMovie mov = node as DEElementFullscreenAuthMovie;

            form.CreateHeader("Fullscreen Movie");

            form.CreateInput("Movie", mov.Movie, delegate (string val) { mov.Movie = val; }, NumberBox.NumberMode.Text);

            form.CreateInput("Movie Start Frame", mov.MovieStart.ToString(), delegate (string val) { mov.MovieStart = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Movie End Frame", mov.MovieEnd.ToString(), delegate (string val) { mov.MovieEnd = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
