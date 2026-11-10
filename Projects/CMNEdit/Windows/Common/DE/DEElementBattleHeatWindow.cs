using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementBattleHeatWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementBattleHeat heat = node as DEElementBattleHeat;

            PropertyEditorBuilder.CreateHeader("Battle Heat");
            PropertyEditorBuilder.CreateInput("Heat Change", heat.HeatChange.ToString(), delegate (string val) { heat.HeatChange = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
