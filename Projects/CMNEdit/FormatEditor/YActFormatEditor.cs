using HActLib;
using HActLib.YAct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public override void Import(string filePath)
        {
            base.Import(filePath);

            var yact = BaseYAct.Read(filePath);

            Form1.Instance.ClearEverything();

            Form1.curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;

            FileContext.Reset();
            FileContext.ActiveFormatEditor = new YActFormatEditor();
            FileContext.IsYAct = true;

            Form1.Instance.hactTabs.TabPages.Remove(Form1.Instance.csvTab);
            Form1.Instance.hactTabs.TabPages.Remove(Form1.Instance.resPage);
            Form1.Instance.hactTabs.TabPages.Remove(Form1.Instance.cutPage);

            Form1.Instance.hactTabs.TabPages[0].Text = "YAct";

            HashSet<YActEffect> parentedEffects = new HashSet<YActEffect>();

            if (yact is YActY2)
            {
                for (int i = 0; i < yact.Characters.Count; i++)
                {
                    YActY2Character yactChara = yact.Characters[i] as YActY2Character;
                    TreeNodeYActCharacter charaNode = new TreeNodeYActCharacter(yactChara);

                    foreach (var anim in yactChara.AnimationData)
                    {
                        charaNode.Nodes.Add(new TreeNodeYActY2Animation(anim));
                    }

                    for (int k = yactChara.UnknownY2[0]; k < yactChara.UnknownY2[0] + yactChara.UnknownY2[1]; k++)
                    {
                        YActEffect yactEffect = yact.Effects[k];
                        parentedEffects.Add(yactEffect);
                        TreeNodeYActEffect yactNode = new TreeNodeYActEffect(yactEffect);

                        charaNode.Nodes.Add(yactNode);
                    }

                    Form1.Instance.nodesTree.Nodes.Add(charaNode);
                }

                for (int i = 0; i < yact.Cameras.Count; i++)
                {
                    YActY2Camera yactCam = yact.Cameras[i] as YActY2Camera;
                    TreeNodeYActCamera camNode = new TreeNodeYActCamera(yactCam);

                    foreach (var anim in yactCam.AnimationData)
                    {
                        camNode.Nodes.Add(new TreeNodeYActY2Animation(anim));
                    }

                    Form1.Instance.nodesTree.Nodes.Add(camNode);
                }
            }
            else
            {
                int cur = 0;

                for (int i = 0; i < yact.Characters.Count; i++)
                {
                    TreeNodeYActCharacter chara = new TreeNodeYActCharacter(yact.Characters[i]);
                    chara.Nodes.Add(new TreeNodeYActCharacterMotion(yact.CharacterAnimations[i]));

                    Form1.Instance.nodesTree.Nodes.Add(chara);
                }

                for (int i = 0; i < yact.Cameras.Count; i++)
                {
                    TreeNodeYActCamera cam = new TreeNodeYActCamera(yact.Cameras[i]);
                    var camNode = new TreeNodeYActCameraMotion(new YActFile() { Buffer = cam.Camera.MTBWFile });
                    camNode.OrigCamera = yact.Cameras[i];
                    cam.Nodes.Add(camNode);
                    Form1.Instance.nodesTree.Nodes.Add(cam);
                }
            }

            for (int i = 0; i < yact.Effects.Count; i++)
            {
                YActEffect yactEffect = yact.Effects[i];

                if (parentedEffects.Contains(yactEffect))
                    continue;

                TreeNodeYActEffect yactNode = new TreeNodeYActEffect(yactEffect);
                Form1.Instance.nodesTree.Nodes.Add(yactNode);
            }

            Form1.Instance.hactDurationPanel.Visible = false;
        }
    }
}
