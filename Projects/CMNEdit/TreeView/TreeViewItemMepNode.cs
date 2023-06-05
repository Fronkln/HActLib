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
                Text = Reflection.GetElementNameByID((node as MepEffectOE).Effect.ElementKind, Form1.curGame).Replace("e_auth_element_", "").Replace("_", " ").ToTitleCase();
            }
        }
    }
}
