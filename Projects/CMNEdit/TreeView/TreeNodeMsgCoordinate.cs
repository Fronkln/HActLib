using YakuzaDataTypes.MSG;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace CMNEdit
{
    internal class TreeNodeMsgCoordinate : TreeNode
    {
        public MsgPositionData Position;

        public TreeNodeMsgCoordinate()
        {
            ContextMenuStrip = Form1.Instance.msgContextEvent;
        }

        public TreeNodeMsgCoordinate(MsgPositionData position)
        {
            Position = position;

            string x = Math.Round(position.Position.x, 3).ToString(CultureInfo.InvariantCulture);
            string y = Math.Round(position.Position.y, 3).ToString(CultureInfo.InvariantCulture);
            string z = Math.Round(position.Position.z, 3).ToString(CultureInfo.InvariantCulture);
            Text = $"({x}, {y}, {z}, Ang: {position.Angle.ToString()})";
        }

        public override object Clone()
        {
            TreeNodeMsgCoordinate cloned = (TreeNodeMsgCoordinate)base.Clone();
            cloned.Position = Position.Copy();

            return cloned;
        }
    }
}
