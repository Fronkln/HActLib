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
using System.Xml;

namespace CMNEdit
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;
        public static string FilePath;

        public static bool IsBep;
        public static bool IsHact;
        public static bool IsOE;

        public static GameVersion curVer;
        public static Game curGame = Game.YLAD; //only used for DE

        public static TreeViewItemNode CopiedNode = null;
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

        private static Node[] EditingResourceCurrentLinkedNodes = null;

        //toggled depending on file
        private TabPage resPage;
        private TabPage cutPage;

        //0 = nodes, 1 res blablabla
        private int currentTab = 0;
        int rowCount = 1;

        private static string folderDir = "";
        private static HActInfo hactInf;

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

            hactInf.Par?.Dispose();


            HActInfo inf = new HActInfo(dialog.SelectedPath);

            if (inf.IsTEV)
                throw new NotImplementedException("TEV unimplemented");

            if (!File.Exists(inf.MainPath))
                return;

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

            hactInf.Par?.Dispose();

            HActInfo inf = new HActInfo(dialog.FileName);

            hactInf = inf;
            folderDir = Path.GetDirectoryName(dialog.FileName);

            ProcessHAct(inf);
        }

        private void ProcessHAct(HActInfo hactInf)
        {

            if (string.IsNullOrEmpty(hactInf.MainPath))
                return;

            ClearEverything();

            // byte[] buf = File.ReadAllBytes(hactInf.MainPath);
            byte[] buf = hactInf.GetCmnBuffer();

            if (buf == null)
                return;

            if (hactInf.Par != null)
                hactInf.Par.Dispose();

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
                IsHact = true;

                //OE/DE HAct
                uint ver = BitConverter.ToUInt32(buf, 0);
                bool isDE = ver == 18;

                RES Res = null;

                if (hactInf.ResourcesPaths.Length > 0)
                    Res = RES.Read(hactInf.ResourcesPaths[0], isDE);

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

            if (hactInf.Par != null)
                if (!hactInf.Par.Disposed)
                    hactInf.Par.Dispose();

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

        public void ProcessSelectedNode(TreeViewItemNode treeNode)
        {
            varPanel.SuspendLayout();

            if (treeNode == null)
            {
                ProcessSpecialSelectedNode();
                return;
            }

            ClearNodeMenu();

            EditingNode = treeNode;

            if (EditingNode == null)
                return;

            Node node = EditingNode.HActNode;

            NodeWindow.Draw(this, EditingNode.HActNode);

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
                  //  if (CMN.IsDE(curVer))
                    //    DENodeConditionFolderWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.CharacterBehavior:
                    if(CMN.IsDE(curVer))
                        DENodeCharacterBehaviorWindow.Draw(this, node);
                    break;

                case AuthNodeCategory.Model_node:
                    DENodeModelWindow.Draw(this, node);
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

            ClearNodeMenu();

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
                ProcessSelectedNode((e.Node as TreeViewItemNode));
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
                            CopiedNode = nodesTree.SelectedNode as TreeViewItemNode;
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

                    if (currentTab == 0 && nodesTree.SelectedNode != null)
                    {
                        TreeViewItemNode node = nodesTree.SelectedNode as TreeViewItemNode;
                        DeleteNode(node);
                    }
                }
            }
        }


        private void DeleteNode(TreeViewItemNode node)
        {
            node.Remove();
        }

        private void PasteNode(TreeViewItemNode pastingNode)
        {
            if (pastingNode == null)
                return;

            TreeViewItemNode parentNode = null;
            TreeViewItemNode hactNode = nodesTree.SelectedNode as TreeViewItemNode;

            if (!IsBep)
            {
                if (hactNode != null && parentNode != hactNode)
                    parentNode = hactNode;
                else
                    parentNode = null;
            }

            TreeViewItemNode newNode = (TreeViewItemNode)pastingNode.Clone();
            TreeNode node = newNode;

            if (parentNode != null)
            {
                parentNode.Nodes.Add(node);
                parentNode.Expand();
            }
            else
                nodesTree.Nodes.Add(node);
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

            foreach (TreeViewItemNode node in GetAllNodesTreeView())
                bep.Nodes.Add(node.HActNode);

            return bep;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yarhl.IO.DataStream stream = null;


            //Write hact
            if (hactInf.Par == null)
            {
                if (IsHact)
                {
                    BaseCMN cmn = (IsOE ? GenerateOEHAct() : GenerateHAct());
                    GenerateBaseInfo(cmn);
                    if (IsOE)
                        OECMN.Write(cmn as OECMN, Path.Combine(folderDir, $"cmn.bin"));
                    else
                        CMN.Write(cmn as CMN, hactInf.MainPath);

                    if (hactInf.ResourcesPaths.Length > 0)
                    {
                        RES newRes = new RES();

                        foreach (TreeViewItemResource res in resTree.Nodes)
                            newRes.Resources.Add(res.Resource);

                        RES.Write(newRes, hactInf.ResourcesPaths[0], CMN.IsDE(curVer));
                    }
                }
                else
                    BEP.Write(GenerateBep(), FilePath, curVer);
            }
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
                }
            }
        }

        public NodeElement[] GetCurrentAuthAllElements()
        {
            return GetAllNodes().Where(x => x.Category == AuthNodeCategory.Element).Cast<NodeElement>().ToArray();
        }

        private Node[] GetAllNodes()
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

                case ResourceType.Asset:
                    return nodesToFind.Where(x => x.Category == AuthNodeCategory.Motion_model).ToArray();

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
                if (IsOE) ;
                //OECMN.Write(OEHAct, dialog.FileName);
                else;
                //CMN.Write(HAct, dialog.FileName);
            }
            else;
            //BEP.Write(Bep, dialog.FileName, curVer);
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

            IsHact = false;
            IsOE = false;
            IsBep = true;

            curGame = (Game)targetGameCombo.SelectedIndex;
            curVer = CMN.GetVersionForGame(curGame);

            BEP Bep = BEP.Read(dialog.FileName, curGame);
            FilePath = dialog.FileName;

            hactTabs.TabPages[0].Text = "BEP";
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            hactDurationPanel.Visible = false;

            resTree.Nodes.Clear();

            foreach (Node node in Bep.Nodes)
                nodesTree.Nodes.Add(new TreeViewItemNode(node));
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
            CopiedNode = m_rightClickedNode;
        }

        private void pasteNodeCTRLVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteNode(CopiedNode);
        }

        private TreeViewItemNode m_rightClickedNode = null;
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

            node.Remove();
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
    }
}

