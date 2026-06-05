using Microsoft.VisualBasic.Logging;
using YakuzaDataTypes.MSG;
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
    internal class TreeNodeMsgPropertyOOE : TreeNode
    {
        public MsgPropertyOOE Property;

        public TreeNodeMsgPropertyOOE()
        {
            ContextMenuStrip = Form1.Instance.msgContextProperty;
        }

        public TreeNodeMsgPropertyOOE(MsgPropertyOOE property)
        {
            ContextMenuStrip = Form1.Instance.msgContextProperty;

            Property = property;
            /*
            byte type1 = (byte)((ushort)property.PropType >> 8);
            byte type2 = (byte)((ushort)property.PropType & 0xFF);
            string typeFormat = $"{type1:D}-{type2:D}";

            var propEnumType = typeof(MsgPropTypeUnknown);

            string name = Enum.GetName(propEnumType, (ushort)property.PropType);

            switch (name)
            {
                default:
                    if (name != null)
                        Text = Regex.Replace(name.Replace("MsgPropOOE", ""), @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    else
                        Text = Regex.Replace("Property", @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    break;
            }
            */


        }

        public override object Clone()
        {
            TreeNodeMsgPropertyOOE cloned = (TreeNodeMsgPropertyOOE)base.Clone();
            cloned.Property = Property.Copy();

            /*
            if(cloned.Property is MsgPropBranching)
            {
                var newBranch = (MsgPropBranching)cloned.Property;
                newBranch.BranchingEvent = (Property as  MsgPropBranching).BranchingEvent;
            }

            if (cloned.Property is MsgPropScenarioBranching)
            {
                var newBranch = (MsgPropScenarioBranching)cloned.Property;
                newBranch.BranchingEvent = (Property as MsgPropScenarioBranching).BranchingEvent;
            }
            */

            return cloned;
        }
    }
}
