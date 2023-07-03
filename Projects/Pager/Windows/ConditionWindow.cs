using System;
using System.Linq;
using HActLib;

namespace Pager
{
    internal static class ConditionWindow
    {
        public static void Draw(Pager form, TreeViewItemCondition cond)
        {
            form.CreateHeader("Condition Information");

            switch((ConditionType)cond.Condition.ConditionID)
            {
                case ConditionType.enemy_num:
                    ConditionEnemyNum enemyCountCond = cond.Condition as ConditionEnemyNum;


                    form.CreateInput("Count: ", enemyCountCond.Count.ToString(), delegate (string val)
                    {
                        enemyCountCond.Count = int.Parse(val);

                    }, NumberBox.NumberMode.Int);
                    break;

                case ConditionType.page_play_count:
                    ConditionPagePlayCount playCountCond = cond.Condition as  ConditionPagePlayCount;


                    form.CreateInput("Count: ", playCountCond.PlayCount.ToString(), delegate (string val)
                    {
                        playCountCond.PlayCount = uint.Parse(val);

                    }, NumberBox.NumberMode.UInt);
                    break;


                case ConditionType.hact_condition_flag:

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

            }
        }
    }
}
