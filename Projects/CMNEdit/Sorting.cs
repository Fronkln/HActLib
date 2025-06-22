using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    public static class Sorting
    {
        public static void SortTreeViewByStart(TreeView treeView)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                SortNodeRecursively(node);
            }

            // Optional: sort top-level nodes too
            SortNodeCollection(treeView.Nodes);
        }

        static void SortNodeRecursively(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                SortNodeRecursively(child);
            }

            SortNodeCollection(node.Nodes);
        }

        static void SortNodeCollection(TreeNodeCollection nodes)
        {
            // Copy to a list
            var sortedList = nodes.Cast<TreeNode>()
                .OrderBy(n => GetStartTime(n))
                .ThenBy(n => n.Name)
                .ToList();

            // Clear and re-add in order
            nodes.Clear();
            nodes.AddRange(sortedList.ToArray());
        }

       private static float GetStartTime(TreeNode node)
        {
            TreeViewItemNode hactNode;

            if ((hactNode = node as TreeViewItemNode) != null)
            {
                if (hactNode.HActNode.Category == AuthNodeCategory.Element)
                    return (hactNode.HActNode as NodeElement).Start;
                else
                    return 0;
            }

            return 0;
        }

        //returns a copy with filtered ones
        //for the auth segmetnter
        public static TreeNode FilterNodesByRange(TreeView treeView, float lastCut, float start, float end, bool adjustRelative = true)
        {
            float length = end - start;

            TreeNode Iterate(TreeNode node)
            {
                TreeViewItemNode nodeH = node as TreeViewItemNode;

                if (nodeH == null)
                    return null;

                if(nodeH.HActNode as NodeElement != null)
                {
                    NodeElement nodeHElement = nodeH.HActNode as NodeElement;

                    //heuteristic
                    float multiPartLimit = 30;

                    //multi-cut node
                    if (lastCut > -1 && nodeHElement.Start + multiPartLimit <= nodeHElement.Start && nodeHElement.Start < start && nodeHElement.End < end)
                    {
                        ;
                    }
                    else if (nodeHElement.Start < start || nodeHElement.Start > end)
                        return null;

                    if (adjustRelative)
                    {
                        nodeHElement.Start = nodeHElement.Start - start;

                        if (nodeHElement.End > end)
                            nodeHElement.End = end;
                    }
                }

                TreeViewItemNode nodeNew = (TreeViewItemNode)node.Clone();
                nodeNew.HActNode.Guid = nodeH.HActNode.Guid;
                nodeNew.Nodes.Clear();

                foreach(TreeNode child in node.Nodes)
                {
                    if (child as TreeViewItemNode == null)
                        continue;

                    TreeViewItemNode childNode = child as TreeViewItemNode;
                    TreeViewItemNode result = (TreeViewItemNode)Iterate(child);

                    if (result != null)
                    {
                        nodeNew.Nodes.Add(result);
                        nodeNew.HActNode.Children.Add(result.HActNode);
                    }
                }

                return nodeNew;
            }

            return Iterate(treeView.Nodes[0]);
        }
    }
}
