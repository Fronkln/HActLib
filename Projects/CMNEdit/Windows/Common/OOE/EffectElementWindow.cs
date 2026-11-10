using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class EffectElementWindow
    {
        public static void Draw(Form1 form, EffectElement element)
        {
            PropertyEditorBuilder.CreateHeader("Element Data");

            PropertyEditorBuilder.CreateInput("Start", element.Start.ToString(), delegate (string val) { element.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("End", element.End.ToString(), delegate (string val) { element.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Element Flags", element.ElementFlags.ToString(), delegate (string val) { element.ElementFlags = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Unknown", element.ElementUnk2.ToString(), delegate (string val) { element.ElementUnk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
