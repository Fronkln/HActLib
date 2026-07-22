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



            string typeFormat = property.Type.ToString("X");

            var propEnumType = typeof(MsgPropTypeOOE);

            string name = Enum.GetName(propEnumType, (ushort)property.Type);

            switch (name)
            {
                default:
                    if (name != null)
                        Text = Regex.Replace(name.Replace("MsgPropOOE", ""), @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    else
                        Text = Regex.Replace("Property", @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    break;
            }

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
