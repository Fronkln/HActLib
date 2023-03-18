using System;
using HActLib;

namespace Pager
{
    public class TreeViewItemPage: TreeNode
    {
        public AuthPage Page;

        public TreeViewItemPage(AuthPage page) : base()
        {
            Page = page;
            Text = page.PageTitleText + $"  ({page.PageIndex})";

            Update();

            foreach (Transition transition in page.Transitions)
                Nodes.Add(new TreeViewItemTransition(page, transition));

            //SetIcon();
        }

        public void Update()
        {
            Text = Page.PageTitleText + $"  ({Page.PageIndex})";
        }
    }
}
