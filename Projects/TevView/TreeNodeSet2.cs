using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using HActLib.OOE;

namespace TevView
{
    internal class TreeNodeSet2 : TreeNode
    {
        public Set2 Set;

        public TreeNodeSet2(Set2 set)
        {
            Set = set;
            Text = set.GetName();

            SetIcon();
        }

        void SetIcon()
        {
            switch(Set.Type)
            {
                case Set2NodeCategory.CameraMotion:
                    SelectedImageIndex = 1;
                    ImageIndex = 1;
                    break;
                case Set2NodeCategory.PathMotion:
                    SelectedImageIndex = 1;
                    ImageIndex = 1;
                    break;
                case Set2NodeCategory.ModelMotion:
                    SelectedImageIndex = 1;
                    ImageIndex = 1;
                    break;
                case Set2NodeCategory.Element:
                    switch(Set.EffectID)
                    {
                        case EffectID.Sound:
                            SelectedImageIndex = 5;
                            ImageIndex = 5;
                            break;
                        case EffectID.Particle:
                            SelectedImageIndex = 7;
                            ImageIndex = 7;
                            break;
                    }
                    break;
            }
        }
    }
}
