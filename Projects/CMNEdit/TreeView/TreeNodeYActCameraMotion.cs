using HActLib.YAct;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeYActCameraMotion : TreeNode
    {
        public YActCamera OrigCamera;
        public YActFile File;

        public TreeNodeYActCameraMotion()
        {

        }

        public TreeNodeYActCameraMotion(YActFile fileImport)
        {
            File = fileImport;
            Text = "Camera Animation (MTBW)";
            ImageIndex = 5;
            SelectedImageIndex = 5;
        }
    }
}
