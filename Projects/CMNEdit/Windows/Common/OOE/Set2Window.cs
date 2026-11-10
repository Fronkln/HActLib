using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class Set2Window
    {
        public static void Draw(Form1 form, Set2 set)
        {
            PropertyEditorBuilder.CreateHeader("Set 2");

            PropertyEditorBuilder.CreateInput("Type", set.Type.ToString(), null, NumberBox.NumberMode.Text, true);
            PropertyEditorBuilder.CreateInput("Effect ID", set.EffectID.ToString(), null, NumberBox.NumberMode.Text, true);

            PropertyEditorBuilder.CreateInput("Resource", set.Resource.ToString(), delegate (string val) { set.Resource = val; });

            PropertyEditorBuilder.CreateInput("Start", set.Start.ToString(), delegate (string val) { set.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("End", set.End.ToString(), delegate (string val) { set.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
