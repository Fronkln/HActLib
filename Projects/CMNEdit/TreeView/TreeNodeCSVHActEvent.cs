using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeCSVHActEvent : TreeNode
    {
        public CSVHActEvent Event;

        public TreeNodeCSVHActEvent()
        {

        }

        public TreeNodeCSVHActEvent(CSVHActEvent hevent)
        {
            Event = hevent;
            Text = hevent.Type;
        }

        public override object Clone()
        {
            TreeNodeCSVHActEvent cloned = (TreeNodeCSVHActEvent)base.Clone();
            cloned.Event = Event.Copy();

            return cloned;
        }
    }
}
