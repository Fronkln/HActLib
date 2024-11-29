using System;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    public class TreeViewItemTransition : TreeNode
    {
        public AuthPage Page;
        public Transition Transition;

        public TreeViewItemTransition(AuthPage page, Transition transition) : base()
        {
            Page = page;
            Transition = transition;


            if (transition.DestinationPageIndex >= Form1.Instance.AuthPagesDE.Length)
                transition.DestinationPageIndex = -1;

            try
            {
                Text = "Transition to: " + (Transition.DestinationPageIndex == -1 ? "HAct end" : Form1.Instance.AuthPagesDE[Transition.DestinationPageIndex].PageTitleText);
            }
            catch (IndexOutOfRangeException ex)
            {
                Text = "Transition to: HAct end";
                transition.DestinationPageIndex = -1;
            }


            foreach (Condition cond in transition.Conditions)
                Nodes.Add(new TreeViewItemCondition(Transition, cond));

            //SetIcon();
        }
    }
}
