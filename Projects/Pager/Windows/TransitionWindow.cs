using System;
using System.Linq;
using HActLib;

namespace Pager
{
    internal static class TransitionWindow
    {
        public static void Draw(Pager form, TreeViewItemTransition transition)
        {
            form.CreateHeader("Transition Information");

            List<string> options = new List<string>();
            options.Add("HAct end");

            foreach(string str in Pager.DEHact.AuthPages.Select(x => x.PageTitleText))
                options.Add(str);

            form.CreateComboBox("Transition to: ", transition.Transition.DestinationPageIndex + 1, options.ToArray(), delegate(int val) 
            {
                transition.Transition.DestinationPageIndex = val - 1;
                form.ProcessHierarchy();

            });
        }
    }
}
