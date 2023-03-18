using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementDivPlayWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementDivPlay divPlay = node as DEElementDivPlay;

            form.CreateHeader("Div Play");

            form.CreateInput("File Name", divPlay.FileName, delegate(string val) { divPlay.FileName = val; });
        }
    }
}
