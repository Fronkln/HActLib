using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeCSVCharacter : TreeNode
    {
        public CSVCharacter Character;

        public TreeNodeCSVCharacter()
        {

        }

        public TreeNodeCSVCharacter(CSVCharacter chara)
        {
            Character = chara;
            Text = Character.Name;
        }

        public override object Clone()
        {
            TreeNodeCSVCharacter cloned = (TreeNodeCSVCharacter)base.Clone();
            cloned.Character = Character.Copy();

            return cloned;
        }
    }
}
