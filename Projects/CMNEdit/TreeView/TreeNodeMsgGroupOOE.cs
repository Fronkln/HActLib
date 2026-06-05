using YakuzaDataTypes.MSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeMsgGroupOOE : TreeNode
    {
        public MsgGroupOOE Group;

        public TreeNodeMsgGroupOOE()
        {

        }

        public TreeNodeMsgGroupOOE(MsgGroupOOE group)
        {
            Group = group;
            Text = "Group";

            foreach (var mEvent in Group.Subgroups)
                Nodes.Add(new TreeNodeMsgSubGroupOOE(mEvent));
        }

        public override object Clone()
        {
            TreeNodeMsgGroupOOE cloned = (TreeNodeMsgGroupOOE)base.Clone();
            cloned.Group = Group.Copy();

            return cloned;
        }
    }
}
