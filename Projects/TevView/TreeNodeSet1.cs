using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TevView
{
    internal class TreeNodeSet1 : TreeNode
    {
        public ObjectBase Set;

        public TreeNodeSet1(ObjectBase set)
        {
            Set = set;
            Text = set.GetName();

            int num1019 = 0;

            if (set.Set3Object != null)
            {
                                /*
                int curIndex = Array.IndexOf(Form1.TevFile.Effects, set.Set3Object);
                EffectBase curEffect = set.Set3Object;
                bool finish = false;

                /*
                for(int i = startIndex; i < goal; i++)
                {
                    if (i >= Form1.TevFile.Effects.Length)
                        break;

                    TreeNodeEffect effectNode = new TreeNodeEffect(Form1.TevFile.Effects[i]);
                    effectNode.Text += " (Reference) (Effects)";
                    Nodes.Add(effectNode);
                }
                */

                /*
                Debug.Print(startIndex + " " + goal + " " + Form1.TevFile.Effects.Length);


                TreeNodeEffect effectNode = new TreeNodeEffect(set.Set3Object);
                effectNode.Text += " (Reference) (Effects)";
                Nodes.Add(effectNode);
                */
            }

            SetIcon();
        }

        void SetIcon()
        {
            switch(Set.Type)
            {
                case ObjectNodeCategory.HumanOrWeapon:
                    if (Set is ObjectWeapon)
                    {
                        SelectedImageIndex = 6;
                        ImageIndex = 6;
                    }
                    else
                    {
                        SelectedImageIndex = 4;
                        ImageIndex = 4;
                    }
                    break;
                case ObjectNodeCategory.Camera:
                    SelectedImageIndex = 3;
                    ImageIndex = 3;
                    break;
                case ObjectNodeCategory.Bone:
                    SelectedImageIndex = 2;
                    ImageIndex = 2;
                    break;
            }

        }
    }
}
