using System;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    public class TreeViewItemPage : TreeNode
    {
        public AuthPage Page;

        public TreeViewItemPage(AuthPage page) : base()
        {
            Page = page;
            Update();

            foreach (Transition transition in page.Transitions)
                Nodes.Add(new TreeViewItemTransition(page, transition));

            //SetIcon();
        }

        public void Update()
        {
            if(Page.Format > 0)
                Text = Page.PageTitleText + $"  ({Page.PageIndex})";
            else
                Text = Page.PageTitleText;
        }
    }
}
