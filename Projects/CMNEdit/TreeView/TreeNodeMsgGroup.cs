using MsgLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeMsgGroup : TreeNode
    {
        public MsgGroup Group;

        public TreeNodeMsgGroup(MsgGroup group)
        {
            Group = group;
            Text = "Group";

            foreach (var mEvent in Group.Events)
                Nodes.Add(new TreeNodeMsgEvent(mEvent));
        }
    }
}
