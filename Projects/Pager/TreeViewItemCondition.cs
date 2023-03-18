using System;
using HActLib;

namespace Pager
{
    public class TreeViewItemCondition : TreeNode
    {
        public Transition Transition;
        public Condition Condition;

        public TreeViewItemCondition(Transition trans, Condition cond) : base()
        {
            Transition = trans;
            Condition = cond;

            Text = GetName();
        }


        public string GetName()
        {
            switch((ConditionType)Condition.ConditionID)
            {
                case ConditionType.hact_condition_flag:
                    ConditionHActFlag flag = Condition as ConditionHActFlag;

                    if (flag.ConditionFlagOn == 1 && flag.ConditionFlagOff == 0)
                        return "QTE Success";
                    else if (flag.ConditionFlagOn == 0 && flag.ConditionFlagOff == 1)
                        return "QTE Failure";
                    break;
            }

            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(((ConditionType)Condition.ConditionID).ToString().Replace("_", " "));
        }
    }
}
