﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using HActLib.OOE;

namespace CMNEdit
{
    internal class TreeViewItemExpressionTargetData : TreeNode
    {
        public ExpressionTargetData Data;

        public TreeViewItemExpressionTargetData()
        { 
        }

        public override object Clone()
        {
            TreeViewItemExpressionTargetData cloned = (TreeViewItemExpressionTargetData)base.Clone();
            cloned.Data = Data.Copy();

            return cloned;
        }

        public TreeViewItemExpressionTargetData(ExpressionTargetData dat)
        {
            Data = dat;
            ImageIndex = 17;
            SelectedImageIndex = 17;
            ForeColor = Color.Purple;
            NodeFont = new Font("Arial", 10, FontStyle.Bold);

            Update();
        }

        public void Update()
        {
            Text = Data.FaceTargetID.ToString();
        }
    }
}
