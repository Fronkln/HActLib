using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class BEPFormatEditor : FormatEditorBase
    {
        public virtual bool HasHierarchy()
        {
            return true;
        }

        public override TreeNode[] GetAllNodes()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            void Process(TreeNode node)
            {
                if ((node as TreeViewItemNode) == null)
                    return;

                TreeViewItemNode nodeHAct = (TreeViewItemNode)node;

                if (nodeHAct.HActNode.Category != HActLib.AuthNodeCategory.Element && nodeHAct.HActNode.Category != HActLib.AuthNodeCategory.FolderCondition)
                    return;

                nodes.Add(node);

                foreach (TreeNode child in node.Nodes)
                    Process(child);
            }


            foreach (TreeNode node in Form1.Instance.nodesTree.Nodes)
                Process(node);

            return nodes.ToArray();
        }
    }
}
