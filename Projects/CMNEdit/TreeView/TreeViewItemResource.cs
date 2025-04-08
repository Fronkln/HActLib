using System.Windows.Forms;
using CMNEdit;
using CMNEdit.Windows;
using HActLib;

namespace CMNEdit.Windows
{
    public class TreeViewItemResource : TreeNode
    {
        public Resource Resource;

        public TreeViewItemResource(Resource res)
        {
            Resource = res;
            Text = res.Name + $"({res.Type})";
        }
    }
}
