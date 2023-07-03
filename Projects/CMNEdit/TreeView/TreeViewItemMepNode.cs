using System;
using System.Reflection;
using System.Windows.Forms;
using HActLib;
using HActLib.Internal;

namespace CMNEdit
{
    public class TreeViewItemMepNode : TreeNode
    {
        public MepEffect Node;

        public TreeViewItemMepNode(MepEffect node)
        {
            Node = node;

            if (node is MepEffectY3)
            {
                Text = (node as MepEffectY3).Effect.GetName();
            }
            else
            {
                MepEffectOE oeMep = (node as MepEffectOE);
                Text = TreeViewItemNode.TranslateName(oeMep.Effect);
                int icon = TreeViewItemNode.SetIcon(oeMep.Effect.Category, oeMep.Effect.Category == AuthNodeCategory.Element ? (oeMep.Effect as NodeElement).ElementKind : 0);

                ImageIndex = icon;
                SelectedImageIndex = icon;
            }
        }
    }
}
