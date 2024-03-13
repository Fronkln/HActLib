using HActLib.YAct;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeYActCharacterMotion : TreeNode
    {
        public YActFile File;

        public TreeNodeYActCharacterMotion()
        {

        }

        public TreeNodeYActCharacterMotion(YActFile fileImport)
        {
            File = fileImport;
            Text = "Character Animation (OMT)";
            ImageIndex = 5;
            SelectedImageIndex = 5;
        }
    }
}
