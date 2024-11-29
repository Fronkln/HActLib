using System;
using System.Threading;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
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
            switch(ConditionConvert.GetName(Condition.ConditionID, Form1.curGame))
            {
                case "hact_condition_flag":
                    ConditionHActFlag flag = Condition as ConditionHActFlag;

                    if (flag.ConditionFlagOn == 1 && flag.ConditionFlagOff == 0)
                        return "QTE Success";
                    else if (flag.ConditionFlagOn == 0 && flag.ConditionFlagOff == 1)
                        return "QTE Failure";
                    break;
            }


            string name = ConditionConvert.GetName(Condition.ConditionID, Form1.curGame);

            if (string.IsNullOrEmpty(name))
                name = "Condition " + Condition.ConditionID.ToString();

            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name).ToString().Replace("_", " ");
        }
    }
}
