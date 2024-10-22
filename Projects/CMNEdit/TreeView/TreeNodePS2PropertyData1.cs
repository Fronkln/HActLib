using HActLib;
using HActLib.YAct;
using System;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodePS2PropertyData1 : TreeNode
    {
        public OMTMoveProperty Data1;

        public TreeNodePS2PropertyData1()
        {

        }

        public TreeNodePS2PropertyData1(OMTMoveProperty prop)
        {
            Data1 = prop;
            Text = "Move Property";

            SelectedImageIndex = 1;
            ImageIndex = 1;
        }

        public override object Clone()
        {
            TreeNodePS2PropertyData1 cloned = (TreeNodePS2PropertyData1)base.Clone();
            cloned.Data1 = Data1.Copy();

            return cloned;
        }
    }
}
