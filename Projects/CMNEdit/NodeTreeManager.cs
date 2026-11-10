using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class NodeTreeManager
    {
        public static Node[] FilterNodesBasedOnResource(ResourceType type)
        {
            var nodesToFind = FileContext.ActiveFormatEditor.GetAllNodes().Cast<TreeViewItemNode>().Select(x => x.HActNode);

            if (nodesToFind == null)
                return new Node[0];

            switch (type)
            {
                default:
                    return new Node[0];

                case ResourceType.AssetMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.ModelMotion).ToArray();

                case ResourceType.Character:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.Character).ToArray();

                case ResourceType.CharacterMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.CharacterMotion).ToArray();

                case ResourceType.CameraMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.CameraMotion).ToArray();

                case ResourceType.PathMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.PathMotion).ToArray();
            }

        }

        public static object[] FilterNodesBasedOnResourceOOE(AuthResourceOOEType type)
        {
            AuthNodeOOE[] nodesToFind = FileContext.ActiveFormatEditor.GetAllNodes().Cast<TreeNodeOOEAuthNode>().Select(x => x.Node).ToArray();

            if (nodesToFind == null)
                return new AuthNodeOOE[0];

            switch (type)
            {
                default:
                    return new AuthNodeOOE[0];
                case AuthResourceOOEType.Character:
                    return nodesToFind.Where(x => x.Type == AuthNodeTypeOOE.Character).ToArray();
                case AuthResourceOOEType.ObjectMotion:
                    return nodesToFind.Where(x => x.Type == AuthNodeTypeOOE.Character || x.Type == AuthNodeTypeOOE.Model).Select(x => x.AnimationData.Guid).Cast<object>().ToArray();
                case AuthResourceOOEType.Model:
                    return nodesToFind.Where(x => x.Type == AuthNodeTypeOOE.Model).ToArray();
            }
        }

        public static List<TreeNode> GetTreeInOrder(TreeView treeView)
        {
            List<TreeNode> ordered = new List<TreeNode>();

            void Traverse(TreeNodeCollection nodes)
            {
                foreach (TreeNode node in nodes)
                {
                    ordered.Add(node);
                    if (node.Nodes.Count > 0)
                        Traverse(node.Nodes);
                }
            }

            Traverse(treeView.Nodes);
            return ordered;
        }

        public static TreeNode[] SortNodesByVisualOrder(TreeNode[] inputNodes, TreeView treeView)
        {
            var fullOrder = GetTreeInOrder(treeView);
            return inputNodes
                .OrderBy(node => fullOrder.IndexOf(node))
                .ToArray();
        }
    }
}
