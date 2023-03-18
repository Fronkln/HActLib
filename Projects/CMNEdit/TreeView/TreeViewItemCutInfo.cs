using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    public class TreeViewItemCutInfo : TreeNode
    {
        public GameTick Tick = new GameTick();

        public TreeViewItemCutInfo()
        {

        }

        public TreeViewItemCutInfo(float frame)
        {
            Tick.Frame = frame;
            Text = ToString();
        }

        public override string ToString()
        {
            return "Frame " + Tick.Frame.ToString();
        }
    }
}
