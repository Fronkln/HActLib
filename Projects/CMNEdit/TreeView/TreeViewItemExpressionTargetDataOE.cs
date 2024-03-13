using HActLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeViewItemExpressionTargetDataOE : TreeNode
    {
        public OEExpressionTarget.ExpressionData Data;

        public TreeViewItemExpressionTargetDataOE()
        {
        }

        public TreeViewItemExpressionTargetDataOE(OEExpressionTarget.ExpressionData dat)
        {
            Data = dat;
            ImageIndex = 17;
            SelectedImageIndex = 17;
            ForeColor = Color.Purple;
            NodeFont = new Font("Arial", 10, FontStyle.Bold);
        }

        public void Update()
        {
            Text = "Expression";
        }
    }
}
