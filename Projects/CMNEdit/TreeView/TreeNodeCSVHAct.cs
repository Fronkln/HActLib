using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeCSVHAct : TreeNode
    {
        public CSVHAct HAct;

        public TreeNodeCSVHAct()
        {

        }

        public TreeNodeCSVHAct(CSVHAct hact)
        {
            HAct = hact;
            Text = HAct.Name;
        }

        public override object Clone()
        {
            TreeNodeCSVHAct cloned = (TreeNodeCSVHAct)base.Clone();
            cloned.HAct = HAct.Copy();

            return cloned;
        }
    }
}
