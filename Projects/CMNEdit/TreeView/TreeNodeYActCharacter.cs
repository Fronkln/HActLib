using HActLib.YAct;
using System;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeYActCharacter : TreeNode
    {
        public YActCharacter Character;

        public TreeNodeYActCharacter()
        {

        }

        public TreeNodeYActCharacter(YActCharacter character)
        {
            Character = character;

            Text = character.Name;
            ImageIndex = 6;
            SelectedImageIndex =  6;
        }
    }
}
