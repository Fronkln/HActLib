using CMNEdit.Windows;
using HActLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

        public override void Import(string path)
        {
            base.Import(path);


            HActDir inf = new HActDir();
            inf.Open(path, Form1.Instance.langOverrideBox.Text);

            Form1.hactInf = inf;
            Form1.folderDir = path;

            Form1.Instance.hactTabs.TabPages[0].Text = "CMN";
            Form1.Instance.hactFlagsHolder.Visible = true;
            Form1.Instance.hactStartBox.Visible = true;
            Form1.Instance.hactEndBox.Visible = true;
            FileContext.IsHact = true;

            Form1.Instance.hactFlagsHolder.Visible = true;

            byte[] buf = inf.GetCmnBuffer();

            //OE/DE HAct
            uint ver = BitConverter.ToUInt32(buf, 0);
            bool isDE = ver >= 18 && ver < 25;

            RES Res = null;

            HActDir[] res = Form1.hactInf.GetResources();

            if (!Form1.Instance.hactTabs.TabPages.Contains(Form1.Instance.resPage))
                Form1.Instance.hactTabs.TabPages.Add(Form1.Instance.resPage);

            if (res.Length > 0)
                Form1.Instance.OnImportResFile(res[0].FindResourceFile().Path); ;

            BaseCMN HAct = null;

            if (isDE)
            {
                Form1.curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;
                Form1.curVer = CMN.GetVersionForGame(Form1.curGame);
                HAct = CMN.Read(buf, Form1.curGame);

                Form1.Instance.hactTypeLabel.Visible = true;
                Form1.Instance.typeBox.Visible = true;

                Form1.Instance.OnOpenDEFormat();
            }
            else
            {
                HAct = OECMN.Read(buf, (Game)Form1.Instance.targetGameCombo.SelectedIndex == Game.FOTNS);
                Form1.curVer = GameVersion.Y0_K1;
                Form1.curGame = (Game)Form1.Instance.targetGameCombo.SelectedIndex;
                FileContext.IsOE = true;

                Form1.Instance.cameraInPanel.Visible = true;
            }

            Form1.Instance.Version = HAct.Version;
            Form1.Instance.DisableFrameInfos = HAct.DisableFrameInfo.ToArray();
            Form1.Instance.ResourceCutInfos = HAct.ResourceCutInfo;
            Form1.Instance.CutInfos = HAct.CutInfo.ToList();
            Form1.Instance.cameraInBox.Text = HAct.GetChainCameraIn().ToString(CultureInfo.InvariantCulture);
            Form1.Instance.cameraOutBox.Text = HAct.GetChainCameraOut().ToString(CultureInfo.InvariantCulture);
            Form1.Instance.flagsBox.Text = HAct.GetFlags().ToString();
            Form1.Instance.NodeDrawNum = HAct.GetNodeDrawNum();

            if (isDE)
            {
                CMN hactDE = (HAct as CMN);
                Form1.Instance.typeBox.Text = hactDE.Header.Type.ToString();
                Form1.Instance.SkipPointTickDE = hactDE.Header.SkipPointTick;
                Form1.Instance.SoundInfoDE = hactDE.SoundInfo;
                Form1.Instance.AuthPagesDE = hactDE.AuthPages.ToArray();
                Form1.Instance.AuthPagesDEUnk = hactDE.AuthPageUnk;


                Form1.Instance.addNodeTab.DropDownItems.Add(Form1.Instance.nodeAddTabDE);
            }
            else
                Form1.Instance.SoundInfoOE = (HAct as OECMN).SoundInfo;

            Form1.Instance.nodesTree.Nodes.Add(new TreeViewItemNode(HAct.Root));
            Form1.Instance.hactDurationPanel.Visible = true;
            Form1.Instance.hactStartBox.Text = HAct.HActStart.ToString();
            Form1.Instance.hactEndBox.Text = HAct.HActEnd.ToString();

            if (!Form1.Instance.hactTabs.TabPages.Contains(Form1.Instance.cutPage))
                Form1.Instance.hactTabs.TabPages.Add(Form1.Instance.cutPage);

            if (Res != null)
                foreach (Resource resource in Res.Resources)
                    Form1.Instance.resTree.Nodes.Add(new TreeViewItemResource(resource));
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
