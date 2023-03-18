using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class RGBA32Window
    {
        public static void Draw(Form1 form, RGBA32 col, string name = "Color")
        {
            form.CreateInput(name + " R", col.R.ToString(), delegate (string val) { col.R = uint.Parse(val); });
            form.CreateInput(name + " G", col.G.ToString(), delegate (string val) { col.G = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput(name + " B", col.B.ToString(), delegate (string val) { col.B = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput(name + " A", col.A.ToString(), delegate (string val) { col.A = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
