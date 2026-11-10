using HActLib.YAct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMNEdit
{
    internal class YActFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            var treeNodes = GetAllNodes();
            YActEffect[] effects = treeNodes.Where(x => x is TreeNodeYActEffect).Cast<TreeNodeYActEffect>().Select(x => x.Effect).ToArray();
            Form1.yact.Effects = effects.ToList();
            YActY1.Write(filePath, Form1.yact);
        }
    }
}
