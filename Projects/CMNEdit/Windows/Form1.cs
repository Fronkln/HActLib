using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using HActLib;
using System.IO;
using System.Globalization;
using CMNEdit.Windows.Common.DE;
using CMNEdit.Windows;
using ParLibrary;
using ParLibrary.Converter;
using PIBLib;
using System.Drawing.Text;
using Frame_Progression_GUI;
using System.Collections;
using MWControlSuite;
using System.Text.RegularExpressions;

namespace CMNEdit
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;
        public static string FilePath;

        public static bool IsBep;
        public static bool IsMep;
        public static bool IsHact;
        public static bool IsOE;

        private static MEP Mep;

        public static GameVersion curVer;
        public static Game curGame = Game.YLAD; //only used for DE

        public static TreeNode[] CopiedNode = null;
        public static TreeViewItemResource CopiedResource = null;

        public static TreeViewItemNode EditingNode = null;
        public static TreeViewItemResource EditingResource = null;
        public static TreeViewItemCutInfo EditingCutInfo = null;

        //Hact stuff
        public uint Version;
        public DisableFrameInfo[] DisableFrameInfos;
        public List<float> CutInfos = new List<float>();
        public float[] ResourceCutInfos;
        public float[] SoundInfoDE;
        public uint[] SoundInfoOE;
        public float ChainCameraIn;
        public float ChainCameraOut;
        public uint Flags;
        public int NodeDrawNum;
        public int TypeDE;
        public GameTick SkipPointTickDE;
        public AuthPage[] AuthPagesDE;
        public byte[] AuthPagesDEUnk;

        private static Node[] EditingResourceCurrentLinkedNodes = null;

        //toggled depending on file
        private TabPage resPage;
        private TabPage cutPage;

        //0 = nodes, 1 res blablabla
        private int currentTab = 0;
        int rowCount = 1;

        private static string folderDir = "";
        private static HActDir hactInf;

        public static bool TranslateNames = false;

        public Form1()
        {
            InitializeComponent();
            varPanel.Controls.Clear();
            varPanel.RowCount = 0;
            varPanel.RowStyles.Clear();

            targetGameCombo.Items.AddRange(Enum.GetNames(typeof(Game)));
            targetGameCombo.SelectedIndex = 6;
            resourceTypeBox.Items.AddRange(Enum.GetNames(typeof(ResourceType)));
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            cutPage = hactTabs.TabPages[1];
            resPage = hactTabs.TabPages[2];

            Instance = this;
        }

        private void ClearEverything()
        {
            EditingNode = null;
            EditingResource = null;

            nodesTree.SelNodes.Clear();
            nodesTree.Nodes.Clear();
            resTree.Nodes.Clear();

            varPanel.Controls.Clear();
            varPanel.RowCount = 0;
            varPanel.RowStyles.Clear();
            IsHact = false;
            IsOE = false;
            IsBep = false;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void openCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            HActDir inf = new HActDir();
            inf.Open(dialog.SelectedPath);

            if (inf.FindFile("hact_tev.bin").Valid())
                throw new NotImplementedException("TEV unimplemented");

            hactInf = inf;
            folderDir = dialog.SelectedPath;

            ProcessHAct(inf);
        }

        private void openHActCMNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            HActDir inf = new HActDir();
            inf.Open(dialog.FileName);

            hactInf = inf;
            folderDir = Path.GetDirectoryName(dialog.FileName);

            ProcessHAct(inf);
        }

        private void ProcessHAct(HActDir hactInf)
        {
            ClearEverything();

            // byte[] buf = File.ReadAllBytes(hactInf.MainPath);
            byte[] buf = hactInf.GetCmnBuffer();

            if (buf == null)
                return;

            //OOE HAct TEV
            if (BitConverter.ToString(buf, 0, 4) == "TCAH")
            {
                hactTabs.TabPages[0].Text = "TEV";
                hactTabs.TabPages.Remove(resPage);
                hactTabs.TabPages.Remove(cutPage);

                TEV Tev = TEV.Read(buf);

                hactDurationPanel.Visible = false;
            }
            else
            {
                hactTabs.TabPages[0].Text = "CMN";

                hactStartBox.Visible = true;
                hactEndBox.Visible = true;
                IsBep = false;
                IsMep = false;
                IsHact = true;

                //OE/DE HAct
                uint ver = BitConverter.ToUInt32(buf, 0);
                bool isDE = ver == 18;

                RES Res = null;

                HActDir[] res = hactInf.GetResources();

                if (res.Length > 0)
                    Res = RES.Read(res[0].FindFile("res.bin").Read());


                if (Res != null)
                {
                    if (!hactTabs.TabPages.Contains(resPage))
                        hactTabs.TabPages.Add(resPage);
                }
                else
                    hactTabs.TabPages.Remove(resPage);

                BaseCMN HAct = null;

                if (isDE)
                {
                    curGame = (Game)targetGameCombo.SelectedIndex;
                    curVer = CMN.GetVersionForGame(curGame);
                    HAct = CMN.Read(buf, curGame);
                }
                else
                {
                    HAct = OECMN.Read(buf);
                    curVer = GameVersion.Y0_K1;
                    curGame = (Game)targetGameCombo.SelectedIndex;
                    IsOE = true;
                }

                Version = HAct.Version;
                DisableFrameInfos = HAct.DisableFrameInfo;
                ResourceCutInfos = HAct.ResourceCutInfo;
                CutInfos = HAct.CutInfo.ToList();
                ChainCameraIn = HAct.GetChainCameraIn();
                ChainCameraOut = HAct.GetChainCameraOut();
                Flags = HAct.GetFlags();
                NodeDrawNum = HAct.GetNodeDrawNum();

                if (isDE)
                {
                    CMN hactDE = (HAct as CMN);
                    TypeDE = hactDE.Header.Type;
                    SkipPointTickDE = hactDE.Header.SkipPointTick;
                    SoundInfoDE = hactDE.SoundInfo;
                    AuthPagesDE = hactDE.AuthPages.ToArray();
                    AuthPagesDEUnk = hactDE.AuthPageUnk;
                }
                else
                    SoundInfoOE = (HAct as OECMN).SoundInfo;

                nodesTree.Nodes.Add(new TreeViewItemNode(HAct.Root));
                hactDurationPanel.Visible = true;
                hactStartBox.Text = HAct.HActStart.ToString();
                hactEndBox.Text = HAct.HActEnd.ToString();

                if (!hactTabs.TabPages.Contains(cutPage))
                    hactTabs.TabPages.Add(cutPage);

                resTree.Nodes.Clear();

                if (Res != null)
                    foreach (Resource resource in Res.Resources)
                        resTree.Nodes.Add(new TreeViewItemResource(resource));
            }

            DrawCutInfo();
        }

        public void DrawCutInfo()
        {
            cutInfoTree.Nodes.Clear();

            foreach (float f in CutInfos)
                cutInfoTree.Nodes.Add(new TreeViewItemCutInfo(f));
        }

        public TreeNode[] GetExpanded()
        {
            List<TreeNode> expanded = new List<TreeNode>();

            foreach (TreeNode node in nodesTree.Nodes)
                if (node.IsExpanded)
                    expanded.Add(node);

            return expanded.ToArray();
        }



        public void CreateHeader(string label, float spacing = 0)
        {
            Label label2 = new Label();
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(42, 5);
            label2.Size = new Size(195, 10);
            label2.TabIndex = 0;
            label2.Text = label;

            if (spacing > 0)
            {
                varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, spacing));
                rowCount++;
                varPanel.RowCount = rowCount;

                varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 2);
                varPanel.Controls.Add(CreateText(""), 1, varPanel.RowCount - 2);
            }
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            rowCount++;
            varPanel.RowCount = rowCount;

            varPanel.Controls.Add(label2, 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(CreateText(""), 1, varPanel.RowCount - 2);
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        }

        public Control CreateText(string label, bool left = false)
        {
            Label text = new Label();

            if (!left)
                text.Anchor = AnchorStyles.Right;
            else
                text.Anchor = AnchorStyles.Left;

            text.AutoSize = true;
            text.Size = new Size(58, 15);
            text.TabIndex = 1;
            text.Text = label;

            return text;
        }

        public void CreateSpace(float space)
        {
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, space));
            rowCount++;
            varPanel.RowCount = rowCount;

            varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(CreateText(""), 1, varPanel.RowCount - 2);
        }

        public void CreateSpace(bool big)
        {
            if (big)
                CreateHeader("");
        }

        public TextBox CreateInput(string label, string defaultValue, Action<string> editedCallback, NumberBox.NumberMode mode = NumberBox.NumberMode.Text, bool readOnly = false)
        {
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varPanel.RowCount = rowCount;

            NumberBox input = new NumberBox(mode, editedCallback);
            input.Text = defaultValue;
            input.Size = new Size(200, 15);
            input.ReadOnly = readOnly;

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 2);
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));

            return input;
        }

        public Button CreateButton(string text, Action clicked)
        {
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varPanel.RowCount = rowCount;

            Button input = new Button();
            input.Text = text;
            input.Size = new Size(200, 50);
            input.Click += delegate { clicked?.Invoke(); };

            varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 2);
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));

            return input;
        }

        public Panel CreatePanel(string label, Color color, Action<Color> finished)
        {
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varPanel.RowCount = rowCount;

            Panel input = new Panel();
            input.BorderStyle = BorderStyle.Fixed3D;
            input.Size = new Size(200, 50);
            input.Click += delegate
            {
                CMNEdit.Windows.ColorView myNewForm = new CMNEdit.Windows.ColorView();
                myNewForm.Visible = true;
                myNewForm.Init(input.BackColor, finished);
            };
            input.BackColor = color;

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 2);
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));

            return input;
        }

        public void CreateComboBox(string label, int defaultIndex, string[] items, Action<int> editedCallback)
        {
            if (defaultIndex < 0 || defaultIndex >= items.Length)
            {
#if DEBUG
                //  throw new Exception("Combobox index error");
#endif
                //  defaultIndex = 0;

                if (defaultIndex > 0)
                {
                    List<string> itemsList = items.ToList();

                    while (itemsList.Count - 1 != defaultIndex)
                        itemsList.Add("Unknown");

                    items = itemsList.ToArray();
                }
            }

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varPanel.RowCount = rowCount;

            ComboBox input = new ComboBox();
            input.Items.AddRange(items);
            input.SelectedIndex = defaultIndex;
            input.Size = new Size(200, 15);

            input.SelectedIndexChanged += delegate { editedCallback?.Invoke(input.SelectedIndex); };

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 2);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 2);
            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        }

        public void CreateHexBox(byte[] buf)
        {

            System.ComponentModel.Design.ByteViewer hexBox2 = new System.ComponentModel.Design.ByteViewer();

            hexBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            hexBox2.Location = new System.Drawing.Point(3, 68);
            hexBox2.Name = "hexBox2";
            //hexBox2.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            hexBox2.Size = new System.Drawing.Size(200, 200);
            hexBox2.TabIndex = 3;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            rowCount++;
            varPanel.RowCount = rowCount;

            varPanel.Controls.Add(hexBox2, 1, varPanel.RowCount - 1);
            varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 1);
        }

        void ClearNodeMenu()
        {
            varPanel.Controls.Clear();
            varPanel.RowStyles.Clear();
            rowCount = 1;
        }

        private void ToggleOutputDEGameList(bool toggle)
        {
            targetGameLbl.Enabled = toggle;
            targetGameCombo.Enabled = toggle;
        }

        public void ProcessSelectedNode(TreeNode treeNode)
        {
            varPanel.SuspendLayout();

            ClearNodeMenu();

            if (treeNode == null)
            {
                ProcessSpecialSelectedNode();
                return;
            }

            EditingNode = treeNode as TreeViewItemNode;


            if (EditingNode == null && treeNode as TreeViewItemMepNode == null)
                return;

            if (treeNode as TreeViewItemNode != null)
                EditingNode = treeNode as TreeViewItemNode;

            Node node;

            if (!IsMep)
                node = EditingNode.HActNode;
            else
               node = ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).Effect;

            NodeWindow.Draw(this, node);


            if(IsMep)
            {
                CreateHeader("Mep");
                CreateInput("Bone", ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneName.Text, delegate (string val) { ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneName.Set(val); });
                CreateInput("Bone ID", ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneID.ToString(), delegate (string val) { ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneID =int.Parse(val); }, NumberBox.NumberMode.Int);
            }

            switch (node.Category)
            {
                case AuthNodeCategory.Element:
                    NodeElementWindow.Draw(this, node);
                    DrawElementWindow(node as NodeElement);
                    break;

                case AuthNodeCategory.Asset:
                    if (CMN.IsDE(curVer))
                        DENodeAssetWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.CameraMotion:
                    NodeMotionBaseWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.ModelMotion:
                    NodeMotionBaseWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.CharacterMotion:
                    if (CMN.IsDE(curVer))
                        DENodeCharacterMotionWindow.Draw(this, node);
                    else
                        NodeMotionBaseWindow.Draw(this, node);
                    break;


                case AuthNodeCategory.Character:
                    if (CMN.IsDE(curVer))
                        DENodeCharacterWindow.Draw(this, node);
                    else
                        OENodeCharacterWindow.Draw(this, node);

                    break;

                case AuthNodeCategory.FolderCondition:
                    if (CMN.IsDE(curVer))
                        DENodeConditionFolderWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.CharacterBehavior:
                    if(CMN.IsDE(curVer))
                        DENodeCharacterBehaviorWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.Model_node:
                    DENodeModelWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.Path:
                    CreateHeader("Path");
                    MatrixWindow.Draw(this, (node as NodePathBase).Matrix);
                    break;

            }
            tabControl1.TabPages.Remove(unkInfoPage);


            Be.Windows.Forms.DynamicByteProvider CreateProvider()
            {
                Be.Windows.Forms.DynamicByteProvider provider = new Be.Windows.Forms.DynamicByteProvider(node.unkBytes);

                provider.Changed += delegate
                {
                    if (provider.Bytes.Count == node.unkBytes.Length)
                        node.unkBytes = provider.Bytes.ToArray();
                    else
                        unkBytesBox.ByteProvider = CreateProvider();
                };

                return provider;
            }

            if (node.unkBytes != null && node.unkBytes.Length > 0)
            {
                tabControl1.TabPages.Add(unkInfoPage);

                Be.Windows.Forms.DynamicByteProvider provider = new Be.Windows.Forms.DynamicByteProvider(node.unkBytes);

                unkBytesBox.ByteProvider = provider;
                unkBytesBox.ByteProvider.Changed += delegate
                {
                    if (provider.Bytes.Count == node.unkBytes.Length)
                        node.unkBytes = provider.Bytes.ToArray();
                    else
                        unkBytesBox.ByteProvider = CreateProvider();
                };

            }

            varPanel.VerticalScroll.Value = 0;
            varPanel.ResumeLayout();
        }

        //Example: expression target
        private void ProcessSpecialSelectedNode()
        {
            if (nodesTree.SelectedNode == null)
                return;

            switch (nodesTree.SelectedNode.GetType().Name)
            {
                case "TreeViewItemExpressionTargetData":
                    TreeViewItemExpressionTargetData itemExpDat = nodesTree.SelectedNode as TreeViewItemExpressionTargetData;
                    CreateHeader("Expression Target Data");
                    CreateComboBox("Target", (int)itemExpDat.Data.FaceTargetID, Enum.GetNames<DEFaceTarget>(),
                        delegate (int idx)
                    {
                        itemExpDat.Data.FaceTargetID = (DEFaceTarget)idx;
                        itemExpDat.Update();
                    });

                    CreateButton("Curve", delegate
                    {
                        CurveView myNewForm = new CurveView();
                        myNewForm.Visible = true;
                        myNewForm.Init(itemExpDat.Data.Animation,
                            delegate (byte[] outCurve)
                            {
                                itemExpDat.Data.Animation = outCurve;
                            });
                    });

                    break;
            }
        }

        private void nodesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
                ProcessSelectedNode((e.Node));
        }

        private void nodesTree_AfterSelNodeChanged(object sender, MWControlSuite.MWPropertyEventArgs e)
        {
            if(nodesTree.SelNodes.Count == 1)
                ProcessSelectedNode(nodesTree.SelNode);
        }


        private void nodesTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                //Copy/Paste
                if (e.KeyCode == Keys.C || e.KeyCode == Keys.V)
                {
                    if (currentTab == 0 && nodesTree.SelectedNode != null)
                    {
                        if (e.KeyCode == Keys.C)
                            CopiedNode = nodesTree.SelNodes.Values.Cast<MWTreeNodeWrapper>().Select(x => x.Node).ToArray();
                        else
                            PasteNode(CopiedNode);
                    }
                }
                else if (e.KeyCode == Keys.S)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {

                    if (currentTab == 0 && nodesTree.SelNodes.Count > 0)
                    {
                        TreeNode node = nodesTree.SelectedNode as TreeNode;
                        DeleteSelectedNodes();
                    }
                }
            }
        }


        private void DeleteSelectedNodes()
        {
            MWTreeNodeWrapper[] nodes = nodesTree.SelNodes.Values.Cast<MWTreeNodeWrapper>().ToArray();

            foreach (MWTreeNodeWrapper node in nodes)
                nodesTree.RemoveNode(node);
        }

        private void PasteNode(TreeNode[] pastingNode)
        {
            if (pastingNode == null)
                return;

            if (!IsMep)
            {
                void PasteNode(TreeViewItemNode node)
                {

                    TreeViewItemNode parentNode = null;
                    TreeViewItemNode hactNode = nodesTree.SelNode as TreeViewItemNode;

                    bool bepConditionPaste = hactNode.HActNode.Category == AuthNodeCategory.FolderCondition;

                    if (!IsBep || bepConditionPaste)
                    {
                        if (hactNode != null && parentNode != hactNode)
                            parentNode = hactNode;
                        else
                            parentNode = null;
                    }

                    TreeViewItemNode newNode = (TreeViewItemNode)node.Clone();

                    if (parentNode != null)
                    {
                        parentNode.Nodes.Add(newNode);
                        parentNode.Expand();
                    }
                    else
                        nodesTree.Nodes.Add(newNode);

                    if (bepConditionPaste)
                    {
                        newNode.HActNode.Guid = hactNode.HActNode.Guid;
                    }
                }

                foreach(var node in pastingNode.Where(x=> x as TreeViewItemNode != null))
                {
                    PasteNode((node as TreeViewItemNode));
                }
            }
            else
            {
                /*
                //Mep to mep paste
                if (pastingNode is TreeViewItemMepNode)
                {
                    TreeViewItemMepNode orig = pastingNode as TreeViewItemMepNode;
                    MepEffect cloned = orig.Node.Copy();
                    TreeViewItemMepNode newNode = new TreeViewItemMepNode(cloned);

                    nodesTree.Nodes.Add(newNode);
                } //Mep to hact paste
                else
                {
                    Node hactNode = (pastingNode as TreeViewItemNode).HActNode;

                    if (hactNode.Category != AuthNodeCategory.Element)
                        return;

                    MepEffectOE effectMep = new MepEffectOE();
                    effectMep.Effect = (NodeElement)hactNode.Copy();

                    nodesTree.Nodes.Add(new TreeViewItemMepNode(effectMep));
                }
                */
            }
        }
        private void nodesTree_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void nodesTree_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "HAct cmn files (*.bin)|*.bin|All files (*.*)|*.*";

            DialogResult res = dialog.ShowDialog();


            if (res == DialogResult.OK)
                CMN.Write(GenerateHAct(), dialog.FileName);
        }

        private void hactTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTab = hactTabs.SelectedIndex;
        }

        private void resTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Node[] nodesToFind = GetAllNodes();

            TreeViewItemResource resNode = e.Node as TreeViewItemResource;
            Resource res = resNode.Resource;

            TreeViewItemNode foundNode = (TreeViewItemNode)nodesTree.GetAllNodes().FirstOrDefault(x => (x as TreeViewItemNode).HActNode.Guid == res.NodeGUID);


            if (foundNode != null)
            {
                nodesTree.SelectedNode = foundNode;
                hactTabs.SelectedIndex = 0;
                ProcessSelectedNode(foundNode);
            }

        }
        private void GenerateBaseInfo(BaseCMN cmn)
        {
            cmn.Version = Version;
            cmn.HActStart = Utils.InvariantParse(hactStartBox.Text);
            cmn.HActEnd = Utils.InvariantParse(hactEndBox.Text);
            cmn.CutInfo = CutInfos.ToArray();
            cmn.DisableFrameInfo = DisableFrameInfos;
            cmn.SetChainCameraIn(ChainCameraIn);
            cmn.SetChainCameraOut(ChainCameraOut);
            cmn.ResourceCutInfo = ResourceCutInfos;
            cmn.SetFlags(Flags);
            cmn.SetNodeDrawNum(NodeDrawNum);

            TreeViewItemNode[] nodes = GetAllNodesTreeView();

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

        private CMN GenerateHAct()
        {
            CMN cmn = new CMN();
            cmn.GameVersion = CMN.GetVersionForGame(curGame);
            cmn.SoundInfo = SoundInfoDE;
            cmn.Header.Type = TypeDE;
            cmn.AuthPages = AuthPagesDE.ToList();
            cmn.AuthPageUnk = AuthPagesDEUnk;
            cmn.Header.SkipPointTick = SkipPointTickDE;

            return cmn;
        }

        private OECMN GenerateOEHAct()
        {
            OECMN cmn = new OECMN();
            cmn.SoundInfo = SoundInfoOE;

            return cmn;
        }

        private BEP GenerateBep()
        {
            BEP bep = new BEP();

            List<Node> nodes = new List<Node>();

            void ChildLoop(TreeViewItemNode node)
            {

                foreach (TreeViewItemNode treeChild in node.Nodes.Cast<TreeNode>().Where(x => x is TreeViewItemNode))
                {
                    bep.Nodes.Add(treeChild.HActNode);
                    ChildLoop(treeChild);
                }
            }

            foreach (TreeViewItemNode node in GetAllNodesTreeView())
            {
                bep.Nodes.Add(node.HActNode);
                ChildLoop(node);
            }


            return bep;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yarhl.IO.DataStream stream = null;

            if(IsBep || IsMep)
                if(string.IsNullOrEmpty(FilePath))
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.DefaultExt = (IsBep ? ".bep" : ".mep");
                    dialog.ShowDialog();
                    FilePath = dialog.FileName;
                }



            //Write hact
            if (IsHact)
            {
                if (!hactInf.IsPar)
                {
                    CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;

                    BaseCMN cmn = (IsOE ? GenerateOEHAct() : GenerateHAct());
                    GenerateBaseInfo(cmn);
                    if (IsOE)
                        OECMN.Write(cmn as OECMN, Path.Combine(folderDir, $"cmn.bin"));
                    else
                        CMN.Write(cmn as CMN, hactInf.FindFile("cmn.bin").Path);

                    
                    if (hactInf.GetResources().Length > 0)
                    {
                        RES newRes = new RES();

                        foreach (TreeViewItemResource res in resTree.Nodes)
                            newRes.Resources.Add(res.Resource);

                        RES.Write(newRes, hactInf.GetResources()[0].FindFile("res.bin").Path, CMN.IsDE(curVer));
                    }
                }
            }
            else if(IsBep)
                BEP.Write(GenerateBep(), FilePath, curVer);
            else if(IsMep)
            {
                curGame = (Game)targetGameCombo.SelectedIndex;

                if (Mep.Version == MEPVersion.Y3)
                    return;

                ConvertCurrentMep();
                MEP.Write(Mep, FilePath);
            }

        }


        private void ConvertCurrentMep()
        {
            curGame = (Game)targetGameCombo.SelectedIndex;
            MepEffectOE[] mepNodes = nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).Cast<MepEffectOE>().ToArray();

            if (Mep.Version == MEPVersion.Y5)
            {
                if (curGame >= Game.Y0)
                {
                    foreach (MepEffectOE oe in mepNodes)
                    {
                        oe.OE_ConvertToY0();
                        oe.Effect.OE_ConvertToY0();
                    }

                }
            }
            else
            {
                if (curGame == Game.Y5)
                    foreach (MepEffectOE oe in mepNodes)
                    {
                        oe.OE_ConvertToY5();
                        oe.Effect.OE_ConvertToY5();
                    }
            }

            Mep.Effects = nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).ToList();
            Mep.Version = (curGame == Game.Y5 ? MEPVersion.Y5 : MEPVersion.Y0);
        }


        //ELEMENT WINDOW DRAWING

        public void DrawElementWindow(NodeElement element)
        {
            //use curver to display things accordingly.

            string elemName = HActLib.Internal.Reflection.GetElementNameByID(element.ElementKind, curGame);

            if (curVer == GameVersion.Y0_K1)
            {
                switch (elemName)
                {
                    case "e_auth_element_particle":
                        OEParticleWindow.Draw(this, element);
                        break;
                    case "e_auth_element_damage":
                        OEDamageWindow.Draw(this, element);
                        break;
                    case "e_auth_element_heat_change":
                        OEHeatChangeWindow.Draw(this, element);
                        break;
                    case "e_auth_element_fade":
                        OEFadeWindow.Draw(this, element);
                        break;
                    case "e_auth_element_sound":
                        OENodeElementSEWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_input":
                        OEHActInputWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_input_barrage":
                        OEHActInputBarrageWindow.Draw(this, element);
                        break;
                    case "e_auth_element_face_expression":
                        OEFaceExpressionWindow.Draw(this, element);
                        break;
                    case "e_auth_element_rim_light_scale":
                        OERimlightScaleWindow.Draw(this, element);
                        break;
                    case "e_auth_element_picture":
                        OEPictureWindow.Draw(this, element);
                        break;
                    case "e_auth_element_noise":
                        OENoiseWindow.Draw(this, element);
                        break;
                    case "e_auth_element_subtitles":
                        OESubtitleWindow.Draw(this, element);
                        break;
                    case "e_auth_element_person_caption":
                        OEPersonCaptionWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_branching":
                        OEHActBranchWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_end":
                        OEHActEndWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_stop_end":
                        OEHActStopEndWindow.Draw(this, element);
                        break;
                    case "e_auth_element_gradation":
                        OEGradationWindow.Draw(this, element);
                        break;
                }
            }
            else if (CMN.IsDE(curVer))
            {
                switch (element.ElementKind)
                {
                    //AuthExtended: System Speed
                    case 1337:
                        DECustomElementSystemSpeedWindow.Draw(this, element);
                        break;
                }

                switch (elemName)
                {
                    case "e_auth_element_se":
                        DENodeElementSEWindow.Draw(this, element);
                        break;
                    case "e_auth_element_speech":
                        DEElementSpeechWindow.Draw(this, element);
                        break;
                    case "e_auth_element_ui_fade":
                        DEElementUIFadeWindow.Draw(this, element);
                        break;
                    case "e_auth_element_ui_texture":
                        DEElementUITextureWindow.Draw(this, element);
                        break;
                    case "e_auth_element_particle":
                        DEElementParticleWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_input":
                        DEHActInputWindow.Draw(this, element);
                        break;
                    case "e_auth_element_hact_input_barrage":
                        DEHActInputBarrageWindow.Draw(this, element);
                        break;
                    case "e_auth_element_battle_damage":
                        DENodeBattleDamageWindow.Draw(this, element);
                        break;
                    case "e_auth_element_battle_heat":
                        DEElementBattleHeatWindow.Draw(this, element);
                        break;
                    case "e_auth_element_equip_asset_hide":
                        DEElementHideAssetWindow.Draw(this, element);
                        break;
                    // case "e_auth_element_color_correction":
                    //  DEElementColorCorrectionWindow.Draw(this, element);
                    //  break;
                    case "e_auth_element_rim_flash":
                        DEElementRimflashWindow.Draw(this, element);
                        break;
                    case "e_auth_element_color_correction_mask":
                        DEElementColorCorrectionMaskWindow.Draw(this, element);
                        break;
                    case "e_auth_element_battle_muteki":
                        DEElementTimingInfoWindow.Draw(this, element);
                        break;
                    case "e_auth_element_battle_attack":
                        DEElementBattleAttackWindow.Draw(this, element);
                        break;
                    case "e_auth_element_connect_chara_out":
                        DEElementCharaOutWindow.Draw(this, element);
                        break;
                    case "e_auth_element_stage_warp":
                        DEElementStageWarpWindow.Draw(this, element);
                        break;
                    case "e_auth_element_face_anim":
                        DEElementFaceAnimWindow.Draw(this, element);
                        break;

                    case "e_auth_element_expression_target":
                        DEElementExpressionTargetWindow.Draw(this, element);
                        break;

                    case "e_auth_element_grain_noise":
                        DEElementGrainNoiseWindow.Draw(this, element);
                        break;

                    case "e_auth_element_character_node_scale":
                        DEElementCharacterNodeScaleWindow.Draw(this, element);
                        break;
                    case "e_auth_element_movie":
                        DEElementMovieWindow.Draw(this, element);
                        break;
                    case "e_auth_element_post_effect_gradation":
                        DEElementGradationWindow.Draw(this, element);
                        break;
                    case "e_auth_element_particle_ground":
                        DEElementParticleGroundWindow.Draw(this, element);
                        break;
                    case "e_auth_element_fullscreen_auth_movie":
                        DEElementFullscreenAuthMovieWindow.Draw(this, element);
                        break;
                    case "e_auth_element_equip_asset":
                        DEElementAssetEquipWindow.Draw(this, element);
                        break;
                    case "e_auth_element_speed_control":
                        DEElementSpeedControlWindow.Draw(this, element);
                        break;
                    case "e_auth_element_character_speed":
                        DEElementCharacterSpeedWindow.Draw(this, element);
                        break;
                    case "e_auth_element_div_play":
                        DEElementDivPlayWindow.Draw(this, element);
                        break;
                    case "e_auth_element_character_change":
                        DEElementCharacterChangeWindow.Draw(this, element);
                        break;
                    case "e_auth_element_asset_break_uid":
                        DEElementAssetBreakUIDWindow.Draw(this, element);
                        break;
                    case "e_auth_element_connect_camera":
                        DEElementCameraLinkWindow.Draw(this, element);
                        break;
                    case "e_auth_element_path_offset":
                        CreateHeader("Path Offset");
                        MatrixWindow.Draw(this, (element as DEElementPathOffset).Matrix);
                        break;
                    case "e_auth_element_scenario_timeline":
                        DEElementScenarioTimelineWindow.Draw(this, element);
                        break;

                    case "e_auth_element_camera_shake":
                        DEEElementCameraShakeWindow.Draw(this, element);
                        break;
                    case "e_auth_element_battle_command_special":
                        DEElementBattleCommandSpecialWindow.Draw(this, element);
                        break;
                }
            }
        }

        public NodeElement[] GetCurrentAuthAllElements()
        {
            Node[] allNodes = GetAllNodes();
            return allNodes.Where(x => x.Category == AuthNodeCategory.Element).Cast<NodeElement>().ToArray();
        }

        public Node[] GetAllNodes()
        {

            if (!IsBep && !IsMep)
            {
                List<TreeNode> nodes = new List<TreeNode>();

                void Process(TreeNode node)
                {
                    nodes.Add(node);

                    foreach (TreeNode child in node.Nodes)
                        Process(child);
                }

                Process(nodesTree.Nodes[0]);

                return nodes.Where(x => x is TreeViewItemNode)
                   .Cast<TreeViewItemNode>()
                   .Select(x => x.HActNode)
                   .ToArray();
            }
            else
                return nodesTree.Nodes
                   .Cast<TreeViewItemNode>()
                   .Select(x => x.HActNode)
                   .ToArray();
        }

        private TreeViewItemNode[] GetAllNodesTreeView()
        {
            return nodesTree.Nodes.Cast<TreeNode>()
               .Where(x => x is TreeViewItemNode)
               .Cast<TreeViewItemNode>()
               .ToArray();
        }

        private Node[] FilterNodesBasedOnResource(ResourceType type)
        {

            Node[] nodesToFind = GetAllNodes();

            if (nodesToFind == null)
                return new Node[0];

            switch (type)
            {
                default:
                    return new Node[0];

                case ResourceType.AssetMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.ModelMotion).ToArray();

                case ResourceType.Character:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.Character).ToArray();

                case ResourceType.CharacterMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.CharacterMotion).ToArray();

                case ResourceType.CameraMotion:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.CameraMotion).ToArray();
            }

        }

        private void applyResButton_Click(object sender, EventArgs e)
        {
            if (EditingResource == null)
                return;

            if (EditingResourceCurrentLinkedNodes == null || EditingResourceCurrentLinkedNodes.Length <= 0)
                return;

            EditingResource.Text = resourceNameTextbox.Text;
            EditingResource.Resource.Name = resourceNameTextbox.Text;
            EditingResource.Resource.Type = (ResourceType)resourceTypeBox.SelectedIndex;
            EditingResource.Resource.NodeGUID = EditingResourceCurrentLinkedNodes[linkedNodeBox.SelectedIndex].Guid;

        }

        private void resTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EditingResource = e.Node as TreeViewItemResource;
            resourceNameTextbox.Text = EditingResource.Resource.Name;
            resourceTypeBox.SelectedIndex = (int)EditingResource.Resource.Type;

            EditingResourceCurrentLinkedNodes = FilterNodesBasedOnResource(EditingResource.Resource.Type);

            linkedNodeBox.Items.Clear();
            linkedNodeBox.Items.AddRange(EditingResourceCurrentLinkedNodes.Select(x => x.Name).ToArray());

            Node foundNode = EditingResourceCurrentLinkedNodes.FirstOrDefault(x => x.Guid == EditingResource.Resource.NodeGUID);

            linkedNodeBox.Text = "";

            if (foundNode != null)
                linkedNodeBox.SelectedIndex = Array.IndexOf(EditingResourceCurrentLinkedNodes, foundNode);
            else
                linkedNodeBox.SelectedIndex = -1;

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsBep && !IsHact && !IsOE)
                return;

            SaveFileDialog dialog = new SaveFileDialog();

            if (IsHact)
                dialog.FileName = "cmn.bin";

            if (IsBep)
                dialog.FileName = FilePath;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            //TODO: GENERATE HACT
            if (IsHact)
            {
                CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;

                BaseCMN cmn = (IsOE ? GenerateOEHAct() : GenerateHAct());
                GenerateBaseInfo(cmn);
                if (IsOE)
                    OECMN.Write(cmn as OECMN, Path.Combine(folderDir, $"cmn.bin"));
                else
                    CMN.Write(cmn as CMN, hactInf.FindFile("cmn.bin").Path);
            }
            else
            {
                if (IsBep)
                    BEP.Write(GenerateBep(), dialog.FileName, curVer);
                else if(IsMep)
                {
                    ConvertCurrentMep();
                    MEP.Write(Mep, dialog.FileName);
                }
            }
        }

        private void openBEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            ClearEverything();

            EditingNode = null;
            EditingResource = null;
            m_rightClickedNode = null;

            curGame = (Game)targetGameCombo.SelectedIndex;
            curVer = CMN.GetVersionForGame(curGame);

            BEP Bep = BEP.Read(dialog.FileName, curGame);
            FilePath = dialog.FileName;

            SetBEPMode();

            foreach (Node node in Bep.Nodes)
            {
                nodesTree.Nodes.Add(new TreeViewItemNode(node));
            }

            TreeViewItemNode[] nodes = GetAllNodesTreeView();

            foreach (TreeViewItemNode node in nodes)
            {
                TreeViewItemNode parentNode = nodes.Where(x => x.HActNode.BEPDat.Guid2 == node.HActNode.Guid).FirstOrDefault();

                if (parentNode != null)
                {
                    nodesTree.RemoveNode(node);
                    parentNode.Nodes.Add(node);
                }
            }
        }

        private void openMEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            ClearEverything();

            EditingNode = null;
            EditingResource = null;
            m_rightClickedNode = null;

            IsHact = false;
            IsOE = false;
            IsMep = true;
            IsBep = false;

            curGame = (Game)targetGameCombo.SelectedIndex;
            curVer = CMN.GetVersionForGame(curGame);

            CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;

            Mep = MEP.Read(dialog.FileName);
            FilePath = dialog.FileName;

            hactTabs.TabPages[0].Text = "MEP";
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            hactDurationPanel.Visible = false;

            resTree.Nodes.Clear();

            foreach (MepEffect node in Mep.Effects)
                nodesTree.Nodes.Add(new TreeViewItemMepNode(node));
        }


        private void SetBEPMode()
        {
            IsHact = false;
            IsOE = false;
            IsMep = false;
            IsBep = true;

            hactTabs.TabPages[0].Text = "BEP";
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            hactDurationPanel.Visible = false;

            resTree.Nodes.Clear();
        }

        private NodeElement AddElementOfType(Type t, string name, string kind)
        {
            //Unimplemented
            if (!CMN.IsDE(curVer))
                return null;

            NodeElement elem = (NodeElement)Activator.CreateInstance(t);
            elem.Guid = Guid.NewGuid();
            elem.Name = name;
            elem.Category = AuthNodeCategory.Element;
            elem.ElementKind = HActLib.Internal.Reflection.GetElementIDByName(kind, curGame);
            elem.UpdateTimingMode = 2;
            elem.PlayType = ElementPlayType.Normal;


            TreeViewItemNode node = nodesTree.SelectedNode as TreeViewItemNode;
            TreeViewItemNode parentNode = null;

            TreeNode nodeNew = new TreeViewItemNode(elem);

            if (!IsBep)
            {
                if (node != null)
                    parentNode = node;
                else
                    parentNode = null;

            }
            else
                nodesTree.Nodes.Add(nodeNew);

            if (parentNode != null)
                parentNode.Nodes.Add(nodeNew);

            return elem;
        }

        private NodeElement AddCustomElementOfType(Type t, string name, uint id)
        {
            //Unimplemented
            if (!CMN.IsDE(curVer))
                return null;

            NodeElement elem = (NodeElement)Activator.CreateInstance(t);
            elem.Guid = Guid.NewGuid();
            elem.Name = name;
            elem.Category = AuthNodeCategory.Element;
            elem.ElementKind = id;
            elem.UpdateTimingMode = 2;
            elem.PlayType = ElementPlayType.Normal;

            TreeViewItemNode node = nodesTree.SelectedNode as TreeViewItemNode;
            TreeViewItemNode parentNode = null;

            TreeNode nodeNew = new TreeViewItemNode(elem);

            if (!IsBep)
            {
                if (node != null)
                    parentNode = node;
                else
                    parentNode = null;

            }
            else
            {
                nodesTree.Nodes.Add(nodeNew);
            }

            if (parentNode != null)
                parentNode.Nodes.Add(nodeNew);

            return elem;
        }
        private void damageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement dmg = AddElementOfType(typeof(NodeBattleDamage), "Battle Damage", "e_auth_element_battle_damage");

            if (curVer != GameVersion.DE2)
                dmg.Version = 0;
            else
                dmg.Version = 1;
        }

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement sound = AddElementOfType(typeof(DEElementSE), "Sound", "e_auth_element_se");
        }

        private void qTEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement qte = AddElementOfType(typeof(DEHActInput), "QTE Input", "e_auth_element_hact_input");
        }

        private void qTEBarrageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement qte = AddElementOfType(typeof(DEHActInputBarrage), "QTE Input Barrage", "e_auth_element_hact_input_barrage");
        }

        private void stageWarpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement warp = AddElementOfType(typeof(DEElementStageWarp), "Stage Warp", "e_auth_element_stage_warp");
        }

        private void resTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                //Copy/Paste
                if (e.KeyCode == Keys.C || e.KeyCode == Keys.V)
                {
                    if (currentTab == 2 && resTree.SelectedNode != null)
                    {
                        if (e.KeyCode == Keys.C)
                            CopiedResource = resTree.SelectedNode as TreeViewItemResource;
                        else
                        {
                            if (CopiedResource == null)
                                return;


                            Resource newRes = CopiedResource.Resource.Clone();
                            resTree.Nodes.Add(new TreeViewItemResource(newRes));
                        }
                    }
                }
                else if (e.KeyCode == Keys.S)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (currentTab == 2 && resTree.SelectedNode != null)
                    {
                        TreeViewItemResource node = resTree.SelectedNode as TreeViewItemResource;
                        node.Remove();
                    }
                }
            }
        }

        private void nodeContext_Opening(object sender, CancelEventArgs e)
        {

        }

        private void copyNodeCTRLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopiedNode = nodesTree.SelNodes.Values.Cast<MWTreeNodeWrapper>().Select(x => x.Node).ToArray();
        }

        private void pasteNodeCTRLVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteNode(CopiedNode);
        }

        private TreeNode m_rightClickedNode = null;
        private void nodesTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                m_rightClickedNode = (TreeViewItemNode)nodesTree.GetNodeAt(e.X, e.Y);
        }

        private void hactStartBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void hactStartBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Utils.InvariantParse(hactStartBox.Text);
            }
            catch { hactStartBox.Text = "0"; }
        }

        private void hactEndBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Utils.InvariantParse(hactEndBox.Text);
            }
            catch { hactEndBox.Text = "0"; }
        }

        private void hactEndBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void deleteSelectedNodeOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nodesTree.SelectedNode == null)
                return;

            TreeViewItemNode node = nodesTree.SelectedNode as TreeViewItemNode;

            foreach (Node child in node.HActNode.Children)
            {
                node.HActNode.Parent.Children.Add(child);
            }

            DeleteSelectedNodes();
        }

        private void cutInfoTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EditingCutInfo = e.Node as TreeViewItemCutInfo;

            if (EditingCutInfo == null)
                return;

            cutInfoFrameBox.Text = EditingCutInfo.Tick.Frame.ToString();
        }

        private void cutInfoApplyButton_Click(object sender, EventArgs e)
        {
            if (EditingCutInfo == null)
                return;

            try
            {
                CutInfos[cutInfoTree.Nodes.IndexOf(EditingCutInfo)] = Utils.InvariantParse(cutInfoFrameBox.Text);
            }
            catch
            {
                cutInfoFrameBox.Text = "0";
            }

            DrawCutInfo();
        }

        private void deleteCutInfoButton_Click(object sender, EventArgs e)
        {
            if (EditingCutInfo == null)
                return;

            CutInfos.RemoveAt(cutInfoTree.Nodes.IndexOf(EditingCutInfo));
            DrawCutInfo();
        }

        private void newCutInfoButton_Click(object sender, EventArgs e)
        {
            CutInfos.Add(0);
            DrawCutInfo();
        }
        private void convertMEPWithPibsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsMep)
                return;

            MessageBox.Show("(SLOW) Convert the current open mep file to the selected OE game including pibs. Input the paths carefully.");

            string input = Microsoft.VisualBasic.Interaction.InputBox("Directory where the mep's pibs are",
           "MEP Pib Path",
           "",
           0,
           0);

            if (string.IsNullOrEmpty(input))
                return;

            string output = Microsoft.VisualBasic.Interaction.InputBox("Directory to output to",
           "MEP Export Path",
           "",
           0,
           0);

           ConvertCurrentMep();

            OEParticle[] mepNodes = nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).Cast<MepEffectOE>().Where(x => x.Effect.ElementKind == 2).Select(x => x.Effect).Cast<OEParticle>().ToArray();

            uint[] pibIds = mepNodes.Select(x => x.ParticleID).ToArray();
            string[] pibFiles = Directory.GetFiles(input, "*.pib", SearchOption.AllDirectories);


            List<string> foundPibPaths = new List<string>();
            List<BasePib> mepPibs = new List<BasePib>();

            foreach(string str in pibFiles)
            {
                if (pibIds.Length == pibFiles.Length)
                    break;

                try
                {
                    BasePib pib = PIB.Read(str);

                    if (pibIds.Contains(pib.ParticleID))
                    {
                        foundPibPaths.Add(str);
                        mepPibs.Add(pib);
                    }
                }
                catch
                {

                }
            }

            PibVersion targetVer = PibVersion.Y5;

            switch(curGame)
            {
                case Game.Ishin:
                    targetVer = PibVersion.Ishin;
                    break;
                case Game.Y5:
                    targetVer = PibVersion.Y5;
                    break;
                case Game.Y0:
                    targetVer = PibVersion.Y0;
                    break;
                case Game.YK1:
                    targetVer = PibVersion.Y0;
                    break;
            }

            for(int i = 0; i < mepPibs.Count; i++)
            {
                BasePib converted = PIB.Convert(mepPibs[i], targetVer);

                foreach (BasePibEmitter emitter in converted.Emitters)
                {
                    string dir = new FileInfo(foundPibPaths[i]).Directory.FullName;

                    foreach (string tex in emitter.Textures)
                    {
                        string texPath = Path.Combine(dir, tex);

                        if (File.Exists(texPath))
                            File.Copy(texPath, Path.Combine(output, tex), true);
                    }
                }

                PIB.Write(converted, Path.Combine(output, converted.Name + ".pib"));
            }


            MEP.Write(Mep, Path.Combine(output, Path.GetFileName(FilePath)));
        }

        private void convertMEPToBEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PibVersion GetVersionForGame(Game game)
            {
                switch(game)
                {
                    default:
                        return PibVersion.Y0;
                    case Game.Y5:
                        return PibVersion.Y5;
                    case Game.Ishin:
                        return PibVersion.Ishin;
                    case Game.Y6:
                        return PibVersion.Y6;
                    case Game.YK2:
                        return PibVersion.YK2;
                    case Game.JE:
                        return PibVersion.JE;
                    case Game.YLAD:
                        return PibVersion.YLAD;
                    case Game.LJ:
                        return PibVersion.LJ;
                }
            }


            Game selectedDEGame = curGame = (Game)targetGameCombo.SelectedIndex;

            if (!CMN.IsDEGame(selectedDEGame))
            {
                MessageBox.Show("Select a DE game first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MEPVersion mepVer;
            Game game;
            Dictionary<string, Node> m_createdBones = new Dictionary<string, Node>();

            Node ConvertMepNodeToBep(MepEffectOE mepNode)
            {
                Node node = null;
                node = OE2DECmn.Convert(mepNode.Effect, OECMN.GetCMNVersionForGame(game), selectedDEGame, game);

                return node;
            }

            OpenFileDialog dialog = new OpenFileDialog();
           // dialog.Filter = ".mep|.MEP file";
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            FilePath = null;

            MEP mep = MEP.Read(dialog.FileName);
            mepVer = mep.Version;
            game = mepVer == MEPVersion.Y5 ? Game.Y5 : Game.Y0;

            ClearEverything();
            SetBEPMode();

            foreach(MepEffectOE mepOe in mep.Effects.Cast<MepEffectOE>())
            {
                Node result = ConvertMepNodeToBep(mepOe);
                TreeViewItemNode resultNode = null;

                if (result != null)
                {
                    result.BEPDat.Bone = mepOe.BoneName;
                    resultNode = new TreeViewItemNode(result);
                    nodesTree.Nodes.Add(resultNode);
                }
            }

            if(MessageBox.Show("Convert particle files?", "Conversion", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FolderBrowserDialog pibDir = new FolderBrowserDialog();

                if (pibDir.ShowDialog() != DialogResult.OK)
                    return;

                string pibSearchDir = pibDir.SelectedPath;

                FolderBrowserDialog pibOutputDir = new FolderBrowserDialog();

                if (pibOutputDir.ShowDialog() != DialogResult.OK)
                    return;

                string pibOutputPath = pibOutputDir.SelectedPath;


                OEParticle[] mepNodes = mep.Effects.Cast<MepEffectOE>().Where(x => x.Effect.ElementKind == 2).Select(x => x.Effect).Cast<OEParticle>().ToArray();

                uint puidStartID = uint.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter the starting ID the converted pibs will have",
                "Adjust PUID ID",
                "",
                0,
                0));

                uint[] pibIds = mepNodes.Select(x => x.ParticleID).GroupBy(x => x).Select(y => y.First()).ToArray();
                string[] pibFiles = Directory.GetFiles(pibSearchDir, "*.pib", SearchOption.AllDirectories);

                Dictionary<uint, uint> newPibIDs = new Dictionary<uint, uint>();

                List<string> foundPibPaths = new List<string>();
                List<BasePib> mepPibs = new List<BasePib>();

                foreach (string str in pibFiles)
                {
                    if (pibIds.Length == pibFiles.Length)
                        break;

                    try
                    {
                        BasePib pib = PIB.Read(str);

                        if (pibIds.Contains(pib.ParticleID))
                        {
                            foundPibPaths.Add(str);
                            mepPibs.Add(pib);
                        }
                    }
                    catch
                    {

                    }
                }

                foreach(BasePib pib in mepPibs)
                {
                    BasePib converted = PIB.Convert(pib, GetVersionForGame(selectedDEGame));

                    if (converted == null)
                        continue;

                    uint oldID = converted.ParticleID;
                    uint newID = puidStartID;
                    puidStartID++;

                    converted.ParticleID = newID;
                    newPibIDs[oldID] = newID;

                    PIB.Write(converted, Path.Combine(pibOutputPath, converted.Name + ".pib"));
                }

                foreach(DEElementParticle ptc in GetAllNodesTreeView().Where(x => x.HActNode is DEElementParticle).Select(x => x.HActNode).Cast<DEElementParticle>())
                {
                    if (newPibIDs.ContainsKey(ptc.ParticleID))
                        ptc.ParticleID = newPibIDs[ptc.ParticleID];
                }
            }
           
        }


        private void adjustTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the multiplier\nTiming * multiplier = New Time\n E.G: Node Start: 30 * 2 = 60",
                       "Retarget Node Times",
                       "1",
                       0,
                       0);

            if (string.IsNullOrEmpty(input))
                return;

            float fact = Utils.InvariantParse(input);

            foreach (NodeElement elem in GetCurrentAuthAllElements())
            {
                elem.Start = elem.Start * fact;
                elem.End = elem.End * fact;
            }

            if (!IsBep)
            {
                hactStartBox.Text = (Utils.InvariantParse(hactStartBox.Text) * fact).ToString();
                hactEndBox.Text = (Utils.InvariantParse(hactEndBox.Text) * fact).ToString();
            }

            Refresh();
        }

        private void moveTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the amount of frames you want to move every node, eg: -30, 46",
           "MoveNode Times",
           "30",
           0,
           0);

            if (string.IsNullOrEmpty(input))
                return;

            int frames = int.Parse(input);

            foreach (NodeElement elem in GetCurrentAuthAllElements())
            {
                elem.Start = elem.Start += frames;
                elem.End = elem.End += frames;
            }

            Refresh();
        }

        private void pathOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement path = AddElementOfType(typeof(DEElementPathOffset), "Path Offset", "e_auth_element_path_offset");
        }

        private void faceExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement exp = AddElementOfType(typeof(DEElementFaceAnim), "Face Anim", "e_auth_element_face_anim");
            exp.UpdateTimingMode = 1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.TranslateNames = checkBox1.Checked;
        }

        private void equipAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement equip = AddElementOfType(typeof(DEElementEquipAsset), "Equip Asset", "e_auth_element_equip_asset");
        }

        private void ınvincibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement inv = AddElementOfType(typeof(DETimingInfoMuteki), "Battle Muteki", "e_auth_element_battle_muteki");
        }

        private void characterSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement speed = AddElementOfType(typeof(DEElementCharacterSpeed), "Character Speed", "e_auth_element_character_speed");
        }

        private void systemSpeedEXAuthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement speed = AddCustomElementOfType(typeof(DECustomElementSystemSpeed), "System Speed", 1337);
        }

        private void characterChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement speed = AddElementOfType(typeof(DEElementCharacterChange), "Character Change", "e_auth_element_character_change");
        }

        private void assetBreakUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement breakUID = AddElementOfType(typeof(DEElementAssetBreakUID), "Asset Break UID", "e_auth_element_asset_break_uid");
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void frameProgressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsHact)
                return;

            FrameProgressionWindow curveForm = new  FrameProgressionWindow();
            curveForm.Visible = true;
        }

        private void authPagesDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pager pager = new Pager();
            pager.Visible = true;
        }
    }
}

