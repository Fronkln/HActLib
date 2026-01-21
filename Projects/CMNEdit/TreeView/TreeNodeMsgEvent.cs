using YakuzaDataTypes.MSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeMsgEvent : TreeNode
    {
        public MsgEvent Event;

        public TreeNodeMsgEvent()
        {
            ContextMenuStrip = Form1.Instance.msgContextEvent;
        }

        public TreeNodeMsgEvent(MsgEvent @event)
        {
            Event = @event;

            if (string.IsNullOrEmpty(Event.Value))
                Text = "EMPTY";
            else
                Text = Event.Value;

            foreach (var prop in Event.Properties)
                Nodes.Add(new TreeNodeMsgProperty(prop));

            ContextMenuStrip = Form1.Instance.msgContextEvent;
        }

        public override object Clone()
        {
            TreeNodeMsgEvent cloned = (TreeNodeMsgEvent)base.Clone();
            cloned.Event = Event.Copy();

            return cloned;
        }
    }
}
