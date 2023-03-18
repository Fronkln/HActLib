using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DENodeCharacterMotionWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DENodeCharacterMotion baseMotion = node as DENodeCharacterMotion;

            if (baseMotion == null)
                return;

            form.CreateHeader("Motion Info");

            form.CreateInput("Start", baseMotion.Start.Frame.ToString(), delegate (string val) { baseMotion.Start.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("End", baseMotion.End.Frame.ToString(), delegate (string val) { baseMotion.End.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Motion Frame", baseMotion.MotionTick.Frame.ToString(), delegate (string val) { baseMotion.MotionTick.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Flags", baseMotion.Flags.ToString(), delegate (string val) { baseMotion.Flags = uint.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
