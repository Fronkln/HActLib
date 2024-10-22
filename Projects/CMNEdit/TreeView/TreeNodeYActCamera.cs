using HActLib.YAct;
using System;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeYActCamera : TreeNode
    {
        public YActCamera Camera;

        public TreeNodeYActCamera()
        {

        }

        public TreeNodeYActCamera(YActCamera camera)
        {
            Camera = camera;
            ImageIndex = 4;
            SelectedImageIndex = 4;

            Text = "Camera";
        }
    }
}
