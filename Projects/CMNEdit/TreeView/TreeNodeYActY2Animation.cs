using HActLib.YAct;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeYActY2Animation : TreeNode
    {
        public YActY2AnimationData Animation = new YActY2AnimationData();

        public TreeNodeYActY2Animation()
        {

        }

        public TreeNodeYActY2Animation(YActY2AnimationData data)
        {
            Animation = data;

            Text = $"Animation File ({data.Format})";
            ImageIndex = 5;
            SelectedImageIndex = 5;
        }
    }
}
