using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    public static class HActEventHumanWindow
    {
        public static void Draw(Form1 form, CSVHActEventHuman human)
        {
            PropertyEditorBuilder.CreateHeader("Human (CSV)");
            PropertyEditorBuilder.CreateInput("Unknown", human.Unknown.ToString(), delegate(string value) { human.Unknown = int.Parse(value); }, NumberBox.NumberMode.Int, false);
        }
    }
}
