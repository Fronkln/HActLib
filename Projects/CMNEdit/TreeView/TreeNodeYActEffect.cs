using System;
using System.Windows.Forms;
using HActLib.YAct;

namespace CMNEdit
{
    internal class TreeNodeYActEffect : TreeNode
    {
        public YActEffect Effect;

        public TreeNodeYActEffect()
        {

        }

        public TreeNodeYActEffect(YActEffect effect)
        {
            Effect = effect;
            Text = effect.Name;

            if (string.IsNullOrEmpty(Text))
                Text = "EFFECT";
        }
    }
}
