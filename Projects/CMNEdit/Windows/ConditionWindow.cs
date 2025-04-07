using System;
using System.Linq;
using HActLib;

namespace CMNEdit
{
    internal static class ConditionWindow
    {
        public static void Draw(Pager form, TreeViewItemCondition cond)
        {
            form.CreateHeader("Condition Information");

            switch(ConditionConvert.GetName(cond.Condition.ConditionID, Form1.curGame))
            {
                case "enemy_num":
                    ConditionEnemyNum enemyCountCond = cond.Condition as ConditionEnemyNum;


                    form.CreateInput("Count: ", enemyCountCond.Count.ToString(), delegate (string val)
                    {
                        enemyCountCond.Count = int.Parse(val);

                    }, NumberBox.NumberMode.Int);


                    form.CreateInput("Unknown: ", enemyCountCond.Unknown1.ToString(), delegate (string val)
                    {
                        enemyCountCond.Unknown1 = int.Parse(val);

                    }, NumberBox.NumberMode.Int);

                    form.CreateInput("Unknown: ", enemyCountCond.Unknown2.ToString(), delegate (string val)
                    {
                        enemyCountCond.Unknown2 = int.Parse(val);

                    }, NumberBox.NumberMode.Int);

                    form.CreateInput("Unknown: ", enemyCountCond.Unknown3.ToString(), delegate (string val)
                    {
                        enemyCountCond.Unknown3 = int.Parse(val);

                    }, NumberBox.NumberMode.Int);
                    break;

                case "page_play_count":
                    ConditionPagePlayCount playCountCond = cond.Condition as  ConditionPagePlayCount;


                    form.CreateInput("Count: ", playCountCond.PlayCount.ToString(), delegate (string val)
                    {
                        playCountCond.PlayCount = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);
                    break;

                case "select_menu":
                    ConditionSelectMenu selMenu = cond.Condition as ConditionSelectMenu;

                    form.CreateInput("Choice: ", selMenu.Choice.ToString(), delegate (string val)
                    {
                        selMenu.Choice = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);
                    break;

                case "hact_condition_flag":

                    ConditionHActFlag condHAct = cond.Condition as ConditionHActFlag;

                    form.CreateInput("Condition Flag On: ", condHAct.ConditionFlagOn.ToString(), delegate (string val)
                    {
                        condHAct.ConditionFlagOn = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);

                    form.CreateInput("Condition Flag Off: ", condHAct.ConditionFlagOff.ToString(), delegate (string val)
                    {
                        condHAct.ConditionFlagOff = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);

                    break;

                case "program_param":

                    ConditionProgramParam condprog = cond.Condition as ConditionProgramParam;

                    form.CreateInput("Flag Index", condprog.FlagIndex.ToString(), delegate (string val)
                    {
                        condprog.FlagIndex = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);

                    form.CreateInput("Flag", condprog.Flag.ToString(), delegate (string val)
                    {
                        condprog.Flag = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);

                    break;

            }
        }
    }
}
