using System;
using System.Linq;
using HActLib;

namespace Pager
{
    internal static class ConditionWindow
    {
        public static void Draw(Form1 form, TreeViewItemCondition cond)
        {
            form.CreateHeader("Condition Information");

            switch((ConditionType)cond.Condition.ConditionID)
            {
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
