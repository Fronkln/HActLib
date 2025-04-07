using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    public class TreeNodeOOEAuthNode : TreeNode
    {
        public AuthNodeOOE Node;

        public TreeNodeOOEAuthNode()
        {

        }

        public TreeNodeOOEAuthNode(AuthNodeOOE Node)
        {
            this.Node = Node;

            foreach (AuthNodeOOE child in Node.Children)
            {
                TreeNodeOOEAuthNode childNode = new TreeNodeOOEAuthNode(child);
                Nodes.Add(childNode);
            }

            foreach(EffectBase effect in Node.Effects)
                Nodes.Add(new TreeNodeEffect(effect));

            int icon = SetIcon();

            ImageIndex = icon;
            SelectedImageIndex = icon;

            if (Enum.IsDefined(typeof(AuthNodeTypeOOE), (int)Node.Type))
                Text = Node.Type.ToString();
            else
                Text = "Unknown Node Type " + Node.Type.ToString();
        }

        private int SetIcon()
        {
            switch(Node.Type)
            {
                case AuthNodeTypeOOE.Character:
                    return 6;
            }

            return 1;
        }
    }
}
