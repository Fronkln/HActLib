using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YakuzaDataTypes.MSG;

namespace CMNEdit
{
    internal class TreeNodeMsgSubGroupOOE : TreeNode
    {
        public MsgSubGroupOOE SubGroup;

        public TreeNodeMsgSubGroupOOE()
        {

        }

        public TreeNodeMsgSubGroupOOE(MsgSubGroupOOE group)
        {
            SubGroup = group;
            Text = "Subgroup";

            foreach (var mEvent in SubGroup.Events)
                Nodes.Add(new TreeNodeMsgEventOOE(mEvent));
        }

        public override object Clone()
        {
            TreeNodeMsgSubGroupOOE cloned = (TreeNodeMsgSubGroupOOE)base.Clone();
            cloned.SubGroup = SubGroup.Copy();

            return cloned;
        }
    }
}
