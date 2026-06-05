using YakuzaDataTypes.MSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeMsgEventOOE : TreeNode
    {
        public MsgEventOOE Event;

        public TreeNodeMsgEventOOE()
        {
            ContextMenuStrip = Form1.Instance.msgContextEvent;
        }

        public TreeNodeMsgEventOOE(MsgEventOOE @event)
        {
            Event = @event;

            if (string.IsNullOrEmpty(Event.Text))
                Text = "EMPTY";
            else
                Text = Event.Text;

            foreach (var prop in Event.Properties)
                Nodes.Add(new TreeNodeMsgPropertyOOE(prop));

            ContextMenuStrip = Form1.Instance.msgContextEvent;
        }

        public override object Clone()
        {
            TreeNodeMsgEventOOE cloned = (TreeNodeMsgEventOOE)base.Clone();
            cloned.Event = Event.Copy();

            return cloned;
        }
    }
}
