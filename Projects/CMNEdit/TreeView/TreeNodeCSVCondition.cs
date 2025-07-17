using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace CMNEdit
{
    internal class TreeNodeCSVCondition : TreeNode
    {
        public CSVCondition Condition;

        public TreeNodeCSVCondition()
        {

        }

        public TreeNodeCSVCondition(CSVCondition condition)
        {
            Condition = condition;
            Text = Condition.Type.ToString();
        }

        public override object Clone()
        {
            TreeNodeCSVCondition cloned = (TreeNodeCSVCondition)base.Clone();
            cloned.Condition = Condition.Copy();

            return cloned;
        }
    }
}
