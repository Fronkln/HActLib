using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementBattleCommandSpecialWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementBattleCommandSpecial special = node as DEElementBattleCommandSpecial;

            PropertyEditorBuilder.CreateHeader("Battle Special");

            PropertyEditorBuilder.CreateComboBox("Type", (int)special.Type, Enum.GetNames<BattleCommandSpecialID>(), delegate (int idx) { special.Type = (BattleCommandSpecialID)idx; });
            PropertyEditorBuilder.CreateInput("Param 1", special.Param1.ToString(), delegate (string val) { special.Param1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Param 2", special.Param2.ToString(), delegate (string val) { special.Param2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Param 3", special.Param3.ToString(), delegate (string val) { special.Param3 = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Param String", special.ParamString, delegate (string val) { special.ParamString = val; });
        }
    }
}
