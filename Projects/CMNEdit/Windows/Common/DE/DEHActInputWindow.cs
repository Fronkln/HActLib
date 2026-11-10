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
    internal static class DEHActInputWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEHActInput inf = node as DEHActInput;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("QTE Input");

            PropertyEditorBuilder.CreateInput("Input ID", inf.InputID.ToString(), delegate (string val) { inf.InputID = (uint)int.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Condition Flag", inf.CondFlagNo.ToString(), delegate (string val) { inf.CondFlagNo = (uint)int.Parse(val); }, NumberBox.NumberMode.UInt);

            PropertyEditorBuilder.CreateInput("Decide Frame", new GameTick(inf.DecideTick).Frame.ToString(), delegate (string val) { inf.DecideTick = new GameTick(float.Parse(val)).Tick; }, NumberBox.NumberMode.Float);
        }
    }
}
