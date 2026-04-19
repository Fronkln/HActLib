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

        //sometimes there are bogus cuts (aka order, 0, 282, 488, 680, 953, 952 (???)
        //resource cuts are -1 frame (hact end is 952, resource cut says 953)
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


                bool globalNode = false;
                bool inrange = false;


                float nodeStart = nodeH.HActNode.GetStart();

                if(nodeStart != -1)
                {
                    float nodeEnd = nodeH.HActNode.GetEnd();

                    //heuteristic
                    float multiPartLimit = 30;

                    globalNode = nodeStart == 0 && nodeEnd >= end - 1;
                    inrange = nodeStart + multiPartLimit <= nodeStart && nodeStart < start && nodeEnd < end;

                    if(nodeH.HActNode is NodeMotionBase)
                    {
                        float multiPartLimitMotion = 120;
                        inrange = nodeStart < end;

                        //some damn npcs have multiple animation nodes for some reason
                        //i dont feel like solving this elegantly for now. so all their nod will be included
                        if (!inrange)
                            return null;
                    }
                    else
                    {
                        //multi-cut node
                        if (lastCut > -1 && (globalNode || inrange))
                        {
                            ;
                        }
                        else if (nodeStart < start || nodeStart > end)
                            return null;
                    }
                }

                TreeViewItemNode nodeNew = (TreeViewItemNode)node.Clone();
                nodeNew.HActNode.Guid = nodeH.HActNode.Guid;
                nodeNew.Nodes.Clear();


                NodeElement newElem = nodeNew.HActNode as NodeElement;

                if(newElem != null)
                {
                    if (adjustRelative)
                    {

                        if (newElem.End > end)
                            newElem.End = end;

                        if (lastCut > -1)
                        {
                            float newEnd = Utils.ConvertRange(newElem.End, start, end, 0, length);
                            newElem.End = newEnd;
                        }

                        if(!globalNode)
                            newElem.Start = newElem.Start - start;
                    }

                }
                else
                {
                    if(nodeNew.HActNode is NodeMotionBase)
                    {
                        var motion = nodeNew.HActNode as NodeMotionBase;

                        if (adjustRelative)
                        {

                            if (motion.End.Frame > end)
                                motion.End.Frame = end;

                            if (lastCut > -1)
                            {
                                float newEnd = Utils.ConvertRange(motion.End.Frame, start, end, 0, length);
                                motion.End.Frame = newEnd;
                            }

                            if (!globalNode)
                                motion.Start.Frame = motion.Start.Frame - start;
                        }
                    }
                }


                foreach (TreeNode child in node.Nodes)
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
