using HActLib.OOE;
using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TevView
{
    internal class TreeNodeEffect : TreeNode
    {
        public EffectBase Effect;
        
        public TreeNodeEffect(EffectBase effect)
        {
            Effect = effect;
            Text = Effect.ElementKind.ToString();

            SetIcon();
        }

        public void SetIcon()
        {
            switch(Effect.ElementKind)
            {
                case EffectID.Particle:
                    SelectedImageIndex = 7;
                    ImageIndex = 7;
                    break;

                case EffectID.Sound:
                    SelectedImageIndex = 5;
                    ImageIndex = 5;
                    break;
            }
        }
    }
}
