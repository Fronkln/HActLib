using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using HActLib;

namespace CMNEdit
{
    internal class OoeAuthFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            var nodesHierarchy = GenerateOOEAuthHierarchy();
            FileContext.Auth.Nodes = nodesHierarchy;

            foreach (AuthNodeOOE node in FileContext.Auth.AllNodes)
            {
                if (node.Type == AuthNodeTypeOOE.Character || node.Type == AuthNodeTypeOOE.Model)
                {
                    if (node.Type == AuthNodeTypeOOE.Character)
                    {
                        if (!FileContext.Auth.References.Any(x => x.Guid == node.AnimationData.Guid))
                        {
                            AuthReference reference = new AuthReference();
                            reference.Guid = node.AnimationData.Guid;

                            FileContext.Auth.References.Add(reference);
                        }
                    }

                    if (!FileContext.Auth.References.Any(x => x.Guid == node.Guid))
                    {
                        AuthReference reference = new AuthReference();
                        reference.Guid = node.Guid;

                        FileContext.Auth.References.Add(reference);
                    }
                }
            }

            FileContext.Auth.CameraCuts = Form1.Instance.CutInfos;
            Auth.Write(FileContext.Auth, FileContext.FilePath);

            if (!string.IsNullOrEmpty(Form1.Instance.ResPath))
            {
                AuthResOOE newRes = new AuthResOOE();

                foreach (TreeViewItemResourceOOE resource in Form1.Instance.resTree.Nodes)
                    newRes.Resources.Add(resource.Resource);

                AuthResOOE.Write(newRes, Form1.Instance.ResPath);
            }
        }

        private List<AuthNodeOOE> GenerateOOEAuthHierarchy()
        {
            void ChildLoop(TreeNode node)
            {
                TreeNodeOOEAuthNode authNode = node as TreeNodeOOEAuthNode;

                if (authNode == null)
                    return;

                if (authNode.Parent != null)
                {
                    authNode.Node.Parent = (authNode.Parent as TreeNodeOOEAuthNode).Node;
                    authNode.Node.Parent.Children.Add(authNode.Node);
                }

                authNode.Node.Children.Clear();
                authNode.Node.Effects.Clear();

                foreach (TreeNode treeChild in node.Nodes.Cast<TreeNode>())
                {
                    if (treeChild is TreeNodeEffect)
                    {
                        authNode.Node.Effects.Add((treeChild as TreeNodeEffect).Effect);
                    }
                    ChildLoop(treeChild);
                }
            }

            List<AuthNodeOOE> rootNodes = new List<AuthNodeOOE>();

            foreach (TreeNode node in Form1.Instance.nodesTree.Nodes)
            {
                TreeNodeOOEAuthNode authNode = node as TreeNodeOOEAuthNode;

                if (authNode == null)
                    continue;

                ChildLoop(node);
                rootNodes.Add(authNode.Node);
            }

            return rootNodes;
        }
    }
}
