using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using HActLib.OOE;

namespace TevView
{
    internal static class EffectElementWindow
    {
        public static void Draw(Form1 form, EffectElement element)
        {
            form.CreateHeader("Element Data");

            form.CreateInput("Start", element.Start.ToString(), delegate (string val) { element.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", element.End.ToString(), delegate (string val) { element.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Element Flags", element.ElementFlags.ToString(), delegate (string val) { element.ElementFlags = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", element.ElementUnk2.ToString(), delegate (string val) { element.ElementUnk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
