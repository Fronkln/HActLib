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
    internal static class DEHActInputBarrageWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEHActInputBarrage inf = node as DEHActInputBarrage;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Input Barrage");

            PropertyEditorBuilder.CreateInput("Barrage Count", inf.BarrageCount.ToString(), delegate (string val) { inf.BarrageCount = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Input ID", inf.InputID.ToString(), delegate (string val) { inf.InputID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Condition Flag", inf.CondFlagNo.ToString(), delegate (string val) { inf.CondFlagNo = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Decide Frame", new GameTick(inf.DecideTick).Frame.ToString(), delegate (string val) { inf.DecideTick = new GameTick(Utils.InvariantParse(val)).Tick; }, NumberBox.NumberMode.Float);
        }
    }
}
