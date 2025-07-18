using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class HActEventButtonWindow
    {
        public static void Draw(Form1 form, CSVHActEventButton button)
        {
            form.CreateHeader("Button (CSV)");
            form.CreateInput("Unknown", button.Unknown1.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown2.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown3.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown4.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown5.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown6.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", button.Unknown7.ToString(), delegate (string val) { button.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
