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

            byte type1 = (byte)((ushort)property.PropType >> 8);
            byte type2 = (byte)((ushort)property.PropType & 0xFF);
            string typeFormat = $"{type1:D}-{type2:D}";

            bool isY0 = Form1.curGame >= HActLib.Game.Ishin;
            var propEnumType = isY0 ? typeof(MsgPropTypeY0) : typeof(MsgPropTypeY5);

            string name = Enum.GetName(propEnumType, (ushort)property.PropType);

            switch (name)
            {
                default:
                    if (name != null)
                        Text = Regex.Replace(name.Replace("MsgProp", ""), @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    else
                        Text = Regex.Replace("Property", @"(?<!^)(?=[A-Z])", " ") + " (" + typeFormat + ")";
                    break;
                case "CameraParam":

                    var camParam = (MsgPropCameraParam)property;

                    if (camParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCamera))
                        Text = "Camera Param (Position & Look At)";
                    else if(camParam.Flags1.HasFlag(MsgPropCameraParam.Flag1.MoveCameraInterpolated))
                        Text = "Camera Param (Smooth Position & Look At)";
                    else
                        Text = "Camera Param";

                    break;
            }


        }

        public override object Clone()
        {
            TreeNodeMsgProperty cloned = (TreeNodeMsgProperty)base.Clone();
            cloned.Property = Property.Copy();

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

            return cloned;
        }
    }
}
