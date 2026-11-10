using HActLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace CMNEdit
{
    internal class PS2PropFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            OMTProperty propFile = new OMTProperty();

            List<OMTMoveProperty> dat1s = Form1.Instance.nodesTree.Nodes.Cast<TreeNode>().Where(x => x is TreeNodePS2PropertyData1).Cast<TreeNodePS2PropertyData1>().Select(x => x.Data1).ToList();
            List<OMTEffectProperty> dat2s = Form1.Instance.nodesTree.Nodes.Cast<TreeNode>().Where(x => x is TreeNodePS2PropertyData2).Cast<TreeNodePS2PropertyData2>().Select(x => x.Data2).ToList();

            propFile.MoveProperties = dat1s;
            propFile.Effects = dat2s;

            OMTProperty.Write(propFile, filePath);
        }
    }
}
