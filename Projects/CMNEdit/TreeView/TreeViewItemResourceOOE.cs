using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeViewItemResourceOOE : TreeNode
    {
        public AuthResourceOOE Resource;

        public TreeViewItemResourceOOE(AuthResourceOOE res)
        {
            Resource = res;
            Text = Resource.Resource + $"({res.Type})";
        }
    }
}

