using YakuzaDataTypes.MSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace CMNEdit
{
    internal class TreeNodeMsgGroup : TreeNode
    {
        public MsgGroup Group;

        public TreeNodeMsgGroup()
        {

        }

        public TreeNodeMsgGroup(MsgGroup group)
        {
            Group = group;
            Text = "Group";

            foreach (var mEvent in Group.Events)
                Nodes.Add(new TreeNodeMsgEvent(mEvent));
        }

        public override object Clone()
        {
            TreeNodeMsgGroup cloned = (TreeNodeMsgGroup)base.Clone();
            cloned.Group = Group.Copy();

            return cloned;
        }
    }
}
