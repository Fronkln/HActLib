using MsgLib;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging.Effects;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeMsgProperty : TreeNode
    {
        public MsgProp Property;

        public TreeNodeMsgProperty()
        {
            ContextMenuStrip = Form1.Instance.msgContextProperty;
        }

        public TreeNodeMsgProperty(MsgProp property)
        {
            ContextMenuStrip = Form1.Instance.msgContextProperty;

            Property = property;
            Text = Regex.Replace(property.GetType().Name.ToString().Replace("MsgProp", ""), @"(?<!^)(?=[A-Z])", " ") + " (" + property.Type1 + "-" + ((byte)property.PropType).ToString() + ")";
        }

        public override object Clone()
        {
            TreeNodeMsgProperty cloned = (TreeNodeMsgProperty)base.Clone();
            cloned.Property = Property.Copy();

            return cloned;
        }
    }
}
