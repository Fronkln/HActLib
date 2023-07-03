using System;
using System.Collections.Generic;
using System.Linq;
using HActLib;

namespace CMNEdit
{
    internal static class TransitionWindow
    {
        public static void Draw(Pager form, TreeViewItemTransition transition)
        {
            form.CreateHeader("Transition Information");

            List<string> options = new List<string>();
            options.Add("HAct end");

            foreach(string str in Form1.Instance.AuthPagesDE.Select(x => x.PageTitleText))
                options.Add(str);

            form.CreateComboBox("Transition to: ", transition.Transition.DestinationPageIndex + 1, options.ToArray(), delegate(int val) 
            {
                transition.Transition.DestinationPageIndex = val - 1;
                form.ProcessHierarchy();

            });
        }
    }
}
