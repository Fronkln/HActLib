using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TevView
{
    internal static class Set2Window
    {
        public static void Draw(Form1 form, Set2 set)
        {
            form.CreateHeader("Set 2");

            form.CreateInput("Type", set.Type.ToString(), null, NumberBox.NumberMode.Text, true);
            form.CreateInput("Effect ID", set.EffectID.ToString(), null, NumberBox.NumberMode.Text, true);

            form.CreateInput("Resource", set.Resource.ToString(), delegate (string val) { set.Resource = val; });

            form.CreateInput("Start", set.Start.ToString(), delegate (string val) { set.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", set.End.ToString(), delegate (string val) { set.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
