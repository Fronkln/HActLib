using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace CMNEdit
{
    internal class TEVFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            FileContext.Tev.Root = GenerateTEVHierarchy();
            TEV.Write(FileContext.Tev, filePath);

            string csvPath = null;

            if (FileContext.Csv != null)
            {
                if (Form1.curGame == Game.Y3)
                    csvPath = INISettings.Y3CsvPath;
                else if (Form1.curGame == Game.Y4)
                    csvPath = INISettings.Y4CsvPath;

                CSV.Write(FileContext.Csv, csvPath);
            }
        }


        private ObjectBase GenerateTEVHierarchy()
        {
            void ChildLoop(TreeNode node)
            {
                if (node as TreeNodeSet1 == null)
                    return;

                TreeNodeSet1 set1Node = node as TreeNodeSet1;
                set1Node.Set.Children.Clear();

                foreach (TreeNode treeChild in node.Nodes.Cast<TreeNode>())
                {
                    if (treeChild is TreeNodeSet1)
                    {
                        TreeNodeSet1 set1Child = (treeChild as TreeNodeSet1);

                        set1Node.Set.Children.Add(set1Child.Set);
                        set1Child.Set.Parent = set1Node.Set;
                    }
                    else if (treeChild is TreeNodeSet2)
                        set1Node.Set.Children.Add((treeChild as TreeNodeSet2).Set);
                    else if (treeChild is TreeNodeEffect)
                        set1Node.Set.Children.Add((treeChild as TreeNodeEffect).Effect);

                    ChildLoop(treeChild);
                }
            }

            ChildLoop(Form1.Instance.nodesTree.Nodes[0]);

            return (Form1.Instance.nodesTree.Nodes[0] as TreeNodeSet1).Set;
        }
    }
}
