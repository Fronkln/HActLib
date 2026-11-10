using CMNEdit.Windows;
using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CMNEdit
{
    internal class HActFormatEditor : FormatEditorBase
    {
        public override bool HasHierarchy()
        {
            return true;
        }

        public override TreeNode[] GetAllNodes()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            void Process(TreeNode node)
            {
                if ((node as TreeViewItemNode) == null)
                    return;

                TreeViewItemNode nodeHAct = (TreeViewItemNode)node;

                nodes.Add(node);

                foreach (TreeNode child in node.Nodes)
                    Process(child);
            }


            foreach (TreeNode node in Form1.Instance.nodesTree.Nodes)
                Process(node);

            return nodes.ToArray();
        }

        public override void Save(string filePath)
        {
            base.Save(filePath);

            if (!Form1.hactInf.IsPar)
            {
                BaseCMN cmn = (FileContext.IsOE ? GenerateOEHAct() : GenerateDEHAct());
                GenerateBaseInfo(cmn);

                if (FileContext.IsOE)
                    OECMN.Write(cmn as OECMN, filePath);
                else
                    CMN.Write(cmn as CMN, filePath);


                if (!string.IsNullOrEmpty(Form1.Instance.ResPath))
                {
                    RES newRes = new RES();
                    newRes.Version = Form1.curGame >= Game.LADPYIH ? 1 : 0;

                    foreach (TreeViewItemResource res in Form1.Instance.resTree.Nodes)
                        newRes.Resources.Add(res.Resource);

                    RES.Write(newRes, Form1.Instance.ResPath, CMN.IsDE(Form1.curVer));
                }
            }
        }

        private void GenerateBaseInfo(BaseCMN cmn)
        {
            cmn.Version = OECMN.GetCMNVersionForGame(Form1.curGame);
            cmn.HActStart = Utils.InvariantParse(Form1.Instance.hactStartBox.Text);
            cmn.HActEnd = Utils.InvariantParse(Form1.Instance.hactEndBox.Text);
            cmn.CutInfo = Form1.Instance.CutInfos.OrderBy(x => x).ToArray();
            cmn.DisableFrameInfo = Form1.Instance.DisableFrameInfos.OrderBy(x => x.Start).ToList();
            cmn.SetChainCameraIn(Utils.InvariantParse(Form1.Instance.cameraInBox.Text));
            cmn.SetChainCameraOut(Utils.InvariantParse(Form1.Instance.cameraOutBox.Text));
            cmn.ResourceCutInfo = Form1.Instance.ResourceCutInfos;
            cmn.SetFlags(uint.Parse(Form1.Instance.flagsBox.Text));
            cmn.SetNodeDrawNum(Form1.Instance.NodeDrawNum);

            TreeViewItemNode[] nodes = GetAllNodes().Cast<TreeViewItemNode>().ToArray();

            void ChildLoop(TreeViewItemNode node)
            {
                node.HActNode.Children.Clear();

                foreach (TreeViewItemNode treeChild in node.Nodes.Cast<TreeNode>().Where(x => x is TreeViewItemNode))
                {
                    node.HActNode.Children.Add(treeChild.HActNode);
                    ChildLoop(treeChild);
                }
            }

            ChildLoop(nodes[0]);

            cmn.Root = nodes[0].HActNode;
        }

        public BaseCMN GenerateFullHAct()
        {
            BaseCMN genHact;

            if (FileContext.IsOE)
                genHact = GenerateOEHAct();
            else
                genHact = GenerateDEHAct();

            GenerateBaseInfo(genHact);

            return genHact;
        }

        public BaseCMN GenerateBaseHAct()
        {
            BaseCMN genHact;

            if (FileContext.IsOE)
                genHact = GenerateOEHAct();
            else
                genHact = GenerateDEHAct();

            return genHact;
        }

        private CMN GenerateDEHAct()
        {
            CMN cmn = new CMN();
            cmn.GameVersion = CMN.GetVersionForGame(Form1.curGame);
            cmn.SoundInfo = Form1.Instance.SoundInfoDE;
            cmn.Header.Type = int.Parse(Form1.Instance.typeBox.Text.ToString());
            cmn.AuthPages = Form1.Instance.AuthPagesDE.ToList();
            cmn.AuthPageUnk = Form1.Instance.AuthPagesDEUnk;
            cmn.Header.SkipPointTick = Form1.Instance.SkipPointTickDE;

            return cmn;
        }

        private OECMN GenerateOEHAct()
        {
            OECMN cmn = new OECMN();
            cmn.SoundInfo = Form1.Instance.SoundInfoOE;

            return cmn;
        }
    }
}
