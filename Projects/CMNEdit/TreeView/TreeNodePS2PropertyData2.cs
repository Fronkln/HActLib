using HActLib;
using HActLib.YAct;
using System;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodePS2PropertyData2 : TreeNode
    {
        public OMTEffectProperty Data2;

        public TreeNodePS2PropertyData2()
        {

        }

        public TreeNodePS2PropertyData2(OMTEffectProperty prop)
        {
            Data2 = prop;
            Text = $"Effect (Type? {prop.Type})";
            SelectedImageIndex = 7;
            ImageIndex = 7;
        }

        public override object Clone()
        {
            TreeNodePS2PropertyData2 cloned = (TreeNodePS2PropertyData2)base.Clone();
            cloned.Data2 = Data2.Copy();

            return cloned;
        }
    }
}
