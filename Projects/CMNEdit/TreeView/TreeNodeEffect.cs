using HActLib.OOE;
using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal class TreeNodeEffect : TreeNode
    {
        public EffectBase Effect;
       
        public TreeNodeEffect()
        {

        }

        public TreeNodeEffect(EffectBase effect)
        {
            Effect = effect;
            Text = Effect.ElementKind.ToString();

            SetIcon();
        }

        public override object Clone()
        {
            TreeNodeEffect cloned = (TreeNodeEffect)base.Clone();
            cloned.Effect = Effect.Copy();

            return cloned;
        }

        public void SetIcon()
        {
            switch(Effect.ElementKind)
            {
                default:
                    SelectedImageIndex = 1;
                    ImageIndex = 1;
                    break;

                case EffectID.Particle:
                    SelectedImageIndex = 7;
                    ImageIndex = 7;
                    break;

                case EffectID.Sound:
                    SelectedImageIndex = 2;
                    ImageIndex = 2;
                    break;
            }
        }
    }
}
