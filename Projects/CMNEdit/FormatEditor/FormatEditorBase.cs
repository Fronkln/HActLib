using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CMNEdit
{
    internal abstract class FormatEditorBase
    {
        public virtual void Import(string filePath)
        {

        }

        public virtual void Save(string filePath)
        {

        }

        public virtual bool HasHierarchy()
        {
            return false;
        }

        public virtual TreeNode[] GetAllNodes()
        {
            if(!HasHierarchy())
                return Form1.Instance.nodesTree.Nodes.Cast<TreeNode>().ToArray();

            List<TreeNode> nodes = new List<TreeNode>();

            void Process(TreeNode node)
            {
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
