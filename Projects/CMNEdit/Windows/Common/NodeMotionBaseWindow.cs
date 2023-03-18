using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class NodeMotionBaseWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeMotionBase baseMotion = node as NodeMotionBase;

            if (baseMotion == null)
                return;

            form.CreateHeader("Motion Info");

            form.CreateInput("Start", baseMotion.Start.Frame.ToString(), delegate (string val) { baseMotion.Start.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", baseMotion.End.Frame.ToString(), delegate (string val) { baseMotion.End.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Flags", baseMotion.Flags.ToString(), delegate (string val) { baseMotion.Flags = uint.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
