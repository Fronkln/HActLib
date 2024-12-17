using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
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
using MotionLibrary;
using MotionLib;
using HActLib.Internal;
using HActLib.OOE;
using HActLib.YAct;

namespace CMNEdit
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;
        public static string FilePath;

        public static bool IsBep;
        public static bool IsMep;
        public static bool IsHact;
        public static bool IsTev;
        public static bool IsOE;
        public static bool IsYAct;
        public static bool IsPS2Prop;

        private static MEP Mep;

        private static TEV Tev;
        public static CSV Csv;
        public uint TevHActID;
        public static CSVHAct TevCsvEntry;

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

        private static BaseYAct yact;

        private static Node[] EditingResourceCurrentLinkedNodes = null;

        //toggled depending on file
        private TabPage resPage;
        private TabPage cutPage;
        private TabPage csvPage;

        private ToolStripDropDownItem addNodeTab;
        private ToolStripItem nodeAddTabDE;
        private ToolStripItem nodeAddTabOOE;

        private ToolStripDropDownItem advancedTab;
        private ToolStripItem advancedFrameProgressionButton;
        private ToolStripItem disableFrameInfoButton;
        private ToolStripItem authPagesButton;
        private ToolStripItem convertBetweenGamesButton;

        private ToolStripDropDownItem convertTab;

        //0 = nodes, 1 res blablabla
        private int currentTab = 0;

        private static string folderDir = "";
        private static HActDir hactInf;

        public static bool TranslateNames = false;

        private ToolStripItem _convertMepButton;

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
            csvPage = hactTabs.TabPages[3];
            csvPage.Enter += delegate { DrawCSV(); };

            addNodeTab = (ToolStripDropDownItem)appTools.Items[1];
            nodeAddTabDE = addNodeTab.DropDownItems[0];
            nodeAddTabOOE = addNodeTab.DropDownItems[1];

            advancedTab = (ToolStripDropDownItem)appTools.Items[3];
            advancedFrameProgressionButton = advancedTab.DropDownItems[0];
            disableFrameInfoButton = advancedTab.DropDownItems[1];
            authPagesButton = advancedTab.DropDownItems[2];

            convertTab = (ToolStripDropDownItem)advancedTab.DropDownItems[3];
            convertBetweenGamesButton = convertTab.DropDownItems[0];

            convertTab.DropDownItems.Remove(convertBetweenGamesButton);

            hactTabs.TabPages.Remove(csvTab);

            Random rnd = new Random();

            if (rnd.Next(0, 16) == 0)
                HammerTime();

            Instance = this;

            _convertMepButton = advancedButton.DropDownItems[0];

            ClearEverything();
        }

        private void HammerTime()
        {
            try
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer("Misc/hammertime.wav");
                simpleSound.Play();
            }
            catch
            {

            }
        }

        private void ClearEverything()
        {
            SuspendLayout();

            DeleteSelectedNodes();

            EditingNode = null;
            EditingResource = null;

            nodesTree.SelNodes.Clear();
            nodesTree.Nodes.Clear();
            resTree.Nodes.Clear();

            varPanel.Controls.Clear();
            varPanel.RowCount = 0;
            varPanel.RowStyles.Clear();

            ResumeLayout(true);

            IsHact = false;
            IsOE = false;
            IsBep = false;


            addNodeTab.DropDownItems.Remove(nodeAddTabOOE);
            addNodeTab.DropDownItems.Remove(nodeAddTabDE);

            advancedTab.DropDownItems.Remove(advancedFrameProgressionButton);
            advancedTab.DropDownItems.Remove(disableFrameInfoButton);
            advancedTab.DropDownItems.Remove(authPagesButton);
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
            inf.Open(dialog.SelectedPath, langOverrideBox.Text);

            if (inf.FindFile("hact_tev.bin").Valid())
                throw new NotImplementedException("TEV unimplemented");

            hactInf = inf;
            folderDir = dialog.SelectedPath;

            ProcessHAct(inf);

            FilePath = inf.GetCmnPath();
        }

        private void openHActCMNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppRegistry.GetFileOpenPath();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            if (hactInf != null)
                if (hactInf.IsPar)
                    if (hactInf.Par != null)
                        hactInf.Par.Dispose();

            HActDir inf = new HActDir();
            inf.Open(dialog.FileName);

            hactInf = inf;
            folderDir = Path.GetDirectoryName(dialog.FileName);

            ProcessHAct(inf);
        }

        private void ProcessHAct(HActDir hactInf)
        {
            AppRegistry.Root.SetValue("LastFileDir", folderDir);

            ClearEverything();

            curGame = (Game)targetGameCombo.SelectedIndex;

            // byte[] buf = File.ReadAllBytes(hactInf.MainPath);
            byte[] buf = hactInf.GetCmnBuffer();

            if (buf == null)
                return;

            IsBep = false;
            IsMep = false;
            IsHact = false;
            IsTev = false;
            IsOE = false;
            IsYAct = false;
            IsPS2Prop = false;

            hactTabs.TabPages.Remove(csvTab);
            addNodeTab.DropDownItems.Remove(nodeAddTabDE);

            byte[] magicBuf = new byte[4];
            Array.Copy(buf, magicBuf, 4);
            string magic = Encoding.UTF8.GetString(magicBuf);

            FilePath = hactInf.FindFile("hact_tev.bin").Path;


            //OOE HAct TEV
            if (magic == "TCAH")
            {
                string csvPath = "";


                if (curGame == Game.Y3)
                    csvPath = INISettings.Y3CsvPath;
                else if (curGame == Game.Y4)
                    csvPath = INISettings.Y4CsvPath;

                if (string.IsNullOrEmpty(csvPath))
                {
                    HammerTime();

                    if (MessageBox.Show("Looks like you are opening a TEV file for the first time!\nTo proceed, you will need to select the hact_csv.bin location where some information will be saved.", "CSV Needed", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        return;

                    OpenFileDialog dialog = new OpenFileDialog();

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    try
                    {
                        CSV testCsv = CSV.Read(dialog.FileName);

                        if (curGame == Game.Y3)
                            INISettings.Y3CsvPath = dialog.FileName;
                        else if (curGame == Game.Y4)
                            INISettings.Y4CsvPath = dialog.FileName;

                        csvPath = dialog.FileName;

                        Program.SaveINIFile();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid CSV file or failed to read.");
                        return;
                    }
                }

                hactTabs.TabPages[0].Text = "TEV";
                hactTabs.TabPages.Remove(resPage);
                hactTabs.TabPages.Remove(cutPage);
                hactTabs.TabPages.Add(csvPage);

                addNodeTab.DropDownItems.Add(nodeAddTabOOE);

                hactStartBox.Visible = false;
                hactEndBox.Visible = false;
                hactDurationPanel.Visible = false;

                IsTev = true;

                Tev = TEV.Read(buf);
                Csv = CSV.Read(csvPath);

                FileInfo inf = new FileInfo(FilePath);

                try
                {
                    if (inf.Directory.Name == "tmp")
                        TevHActID = uint.Parse(inf.Directory.Parent.Name.Substring(0, 4));
                    else
                        TevHActID = uint.Parse(inf.Directory.Name.Substring(0, 4));
                }
                catch
                {
                    TevHActID = 0;
                }

                TevCsvEntry = Csv.TryGetEntry(TevHActID);

                TreeNodeSet1 root = DrawTEVSet1(Tev.Root);
                nodesTree.Nodes.Add(root);
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

                HActDir[] res = hactInf.GetResources();

                if (res.Length > 0)
                    Res = RES.Read(res[0].FindResourceFile().Read());


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
                    OnOpenDEFormat();
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


                    addNodeTab.DropDownItems.Add(nodeAddTabDE);
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

            saveToolStripMenuItem.Enabled = !hactInf.IsPar;

            OnOpenHAct();
            DrawCutInfo();
        }

        private void ProcessYAct(BaseYAct yact)
        {
            AppRegistry.Root.SetValue("LastFileDir", folderDir);

            ClearEverything();

            curGame = (Game)targetGameCombo.SelectedIndex;

            IsBep = false;
            IsMep = false;
            IsHact = false;
            IsTev = false;
            IsOE = false;
            IsPS2Prop = false;
            IsYAct = true;

            hactTabs.TabPages.Remove(csvTab);
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            hactTabs.TabPages[0].Text = "YAct";

            HashSet<YActEffect> parentedEffects = new HashSet<YActEffect>();

            if(yact is YActY2)
            {
                for (int i = 0; i < yact.Characters.Count; i++)
                {
                    YActY2Character yactChara = yact.Characters[i] as YActY2Character;
                    TreeNodeYActCharacter charaNode = new TreeNodeYActCharacter(yactChara);

                    foreach(var anim in yactChara.AnimationData)
                    {
                        charaNode.Nodes.Add(new TreeNodeYActY2Animation(anim));
                    }

                    for(int k = yactChara.UnknownY2[0]; k < yactChara.UnknownY2[0] + yactChara.UnknownY2[1]; k++)
                    {
                        YActEffect yactEffect = yact.Effects[k];
                        parentedEffects.Add(yactEffect);
                        TreeNodeYActEffect yactNode = new TreeNodeYActEffect(yactEffect);

                        charaNode.Nodes.Add(yactNode);
                    }

                    nodesTree.Nodes.Add(charaNode);
                }

                for (int i = 0; i < yact.Cameras.Count; i++)
                {
                    YActY2Camera yactCam = yact.Cameras[i] as YActY2Camera;
                    TreeNodeYActCamera camNode = new TreeNodeYActCamera(yactCam);

                    foreach(var anim in yactCam.AnimationData)
                    {
                        camNode.Nodes.Add(new TreeNodeYActY2Animation(anim));
                    }

                    nodesTree.Nodes.Add(camNode);
                }
            }
            else
            {
                int cur = 0;

                for (int i = 0; i < yact.Characters.Count; i++)
                {
                    TreeNodeYActCharacter chara = new TreeNodeYActCharacter(yact.Characters[i]);
                    chara.Nodes.Add(new TreeNodeYActCharacterMotion(yact.CharacterAnimations[i]));

                    nodesTree.Nodes.Add(chara);
                }

                for (int i = 0; i < yact.Cameras.Count; i++)
                {
                    TreeNodeYActCamera cam = new TreeNodeYActCamera(yact.Cameras[i]);
                    var camNode = new TreeNodeYActCameraMotion(new YActFile() { Buffer = cam.Camera.MTBWFile });
                    camNode.OrigCamera = yact.Cameras[i];
                    cam.Nodes.Add(camNode);
                    nodesTree.Nodes.Add(cam);
                }
            }

            for (int i = 0; i < yact.Effects.Count; i++)
            {
                YActEffect yactEffect = yact.Effects[i];

                if (parentedEffects.Contains(yactEffect))
                    continue;

                TreeNodeYActEffect yactNode = new TreeNodeYActEffect(yactEffect);
                nodesTree.Nodes.Add(yactNode);
            }

            hactDurationPanel.Visible = false;
        }


        private void ProcessPS2Prop(OMTProperty prop)
        {
            IsBep = false;
            IsMep = false;
            IsHact = false;
            IsTev = false;
            IsOE = false;
            IsPS2Prop = true;
            IsYAct = false;

            hactTabs.TabPages.Remove(csvTab);
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);


            hactTabs.TabPages[0].Text = "Property";


            foreach(var dat1 in prop.MoveProperties)
            {
                TreeNodePS2PropertyData1 propNode = new TreeNodePS2PropertyData1(dat1);
                nodesTree.Nodes.Add(propNode);
            }

            foreach (var dat2 in prop.Effects)
            {
                TreeNodePS2PropertyData2 propNode = new TreeNodePS2PropertyData2(dat2);
                nodesTree.Nodes.Add(propNode);
            }
        }

        public void DrawCutInfo()
        {
            cutInfoTree.Nodes.Clear();

            foreach (float f in CutInfos)
                cutInfoTree.Nodes.Add(new TreeViewItemCutInfo(f));
        }

        public void ReDraw()
        {
            SuspendLayout();

            if (nodesTree.Nodes.Count == 0)
                return;

            if (!IsBep)
            {
                Node root = (nodesTree.Nodes[0] as TreeViewItemNode).HActNode;

                nodesTree.Nodes.Clear();
                nodesTree.Nodes.Add(new TreeViewItemNode(root));
            }
            else
            {
                Node[] nodes = GetAllNodes();

                nodesTree.Nodes.Clear();

                foreach (Node node in nodes)
                    nodesTree.Nodes.Add(new TreeViewItemNode(node));
            }

            ResumeLayout(true);
        }

        public void ClearNodes()
        {
            SuspendLayout();

            DeleteSelectedNodes();

            nodesTree.Nodes.Clear();
            ResumeLayout(true);
        }

        public TreeNode[] GetExpanded()
        {
            List<TreeNode> expanded = new List<TreeNode>();

            foreach (TreeNode node in nodesTree.Nodes)
                if (node.IsExpanded)
                    expanded.Add(node);

            return expanded.ToArray();
        }

        public void CreateHeader(string label, float spacing = 0, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            Label label2 = new Label();
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(42, 5);
            label2.Size = new Size(195, 10);
            label2.TabIndex = 0;
            label2.Text = label;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            varPanel.RowCount++;

            varPanel.Controls.Add(label2, 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(CreateText(""), 1, varPanel.RowCount - 1);
        }

        public Control CreateText(string label, bool left = false, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

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

        public void CreateSpace(float space, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, space));
            varPanel.RowCount++;

            varPanel.Controls.Add(CreateText("", isCsvTree), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(CreateText("", isCsvTree), 1, varPanel.RowCount - 1);
        }

        public void CreateSpace(bool big, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            if (big)
                CreateHeader("", 0, isCsvTree);
        }

        public TextBox CreateInput(string label, string defaultValue, Action<string> editedCallback, NumberBox.NumberMode mode = NumberBox.NumberMode.Text, bool readOnly = false, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

            NumberBox input = new NumberBox(mode, editedCallback);
            input.Text = defaultValue;
            input.Size = new Size(200, 15);
            input.ReadOnly = readOnly;
            input.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            Control text = CreateText(label, false);

            varPanel.Controls.Add(text, 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public Button CreateButton(string text, Action clicked, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            varPanel.RowCount++;

            Button input = new Button();
            input.Text = text;
            input.Size = new Size(200, 50);
            input.Click += delegate { clicked?.Invoke(); };

            varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public Panel CreatePanel(string label, Color color, Action<Color> finished, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

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

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public Panel CreatePanelFI(string label, Color color, Action<HActLib.RGB> finished, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

            Panel input = new Panel();
            input.BorderStyle = BorderStyle.Fixed3D;
            input.Size = new Size(200, 50);
            input.Click += delegate
            {
                CMNEdit.Windows.ColorViewFI myNewForm = new CMNEdit.Windows.ColorViewFI();
                myNewForm.Visible = true;
                myNewForm.Init(input.BackColor, finished);
            };
            input.BackColor = color;

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public void CreateComboBox(string label, int defaultIndex, string[] items, Action<int> editedCallback, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

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
                    {
                        //BIG PROBLEM!

                        if (itemsList.Count > 128)
                        {
                            defaultIndex = itemsList.Count - 1;
                            break;
                        }

                        itemsList.Add("Unknown");
                    }

                    items = itemsList.ToArray();
                }
            }

            ComboBox input = new ComboBox();
            input.Items.AddRange(items);
            input.SelectedIndex = defaultIndex;
            input.Size = new Size(200, 15);

            input.SelectedIndexChanged += delegate { editedCallback?.Invoke(input.SelectedIndex); };

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);
        }

        public void CreateHexBox(byte[] buf, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = this.varPanel;
            else
                varPanel = csvVarPanel;

            System.ComponentModel.Design.ByteViewer hexBox2 = new System.ComponentModel.Design.ByteViewer();

            hexBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            hexBox2.Location = new System.Drawing.Point(3, 68);
            hexBox2.Name = "hexBox2";
            //hexBox2.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            hexBox2.Size = new System.Drawing.Size(200, 200);
            hexBox2.TabIndex = 3;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            varPanel.RowCount++;

            varPanel.Controls.Add(hexBox2, 1, varPanel.RowCount - 1);
            varPanel.Controls.Add(CreateText(""), 0, varPanel.RowCount - 1);
        }

        void ClearNodeMenu()
        {
            varPanel.Controls.Clear();
            varPanel.RowStyles.Clear();
            varPanel.RowCount = 0;
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

            System.Diagnostics.Debug.Print(varPanel.RowStyles.Count.ToString());


            if(IsPS2Prop)
            {
                ProcessSelectedNodePS2Prop();
                return;
            }

            if (IsYAct)
            {
                ProcessSelectedNodeYAct();
                return;
            }

            if (IsTev)
            {
                ProcessSelectedNodeTEV();
                return;
            }

            if (treeNode as TreeViewItemNode == null && treeNode as TreeViewItemMepNode == null)
            {
                ProcessSpecialSelectedNode();

                varPanel.VerticalScroll.Value = 0;
                varPanel.ResumeLayout();

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
            {
                if (curGame > Game.Y4)
                    node = ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).Effect;
                else
                {
                    DrawTEVEffectWindow(((treeNode as TreeViewItemMepNode).Node as MepEffectY3).Effect);
                    varPanel.VerticalScroll.Value = 0;
                    varPanel.ResumeLayout();

                    return;
                }
            }

            NodeWindow.Draw(this, node);


            if (IsMep)
            {
                CreateHeader("Mep");
                CreateInput("Bone", ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneName.Text, delegate (string val) { ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneName.Set(val); });
                CreateInput("Bone ID", ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneID.ToString(), delegate (string val) { ((treeNode as TreeViewItemMepNode).Node as MepEffectOE).BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);
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
                    if (CMN.IsDE(curVer))
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
                    if (provider.Bytes.Count >= node.unkBytes.Length)
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
            varPanel.RowCount++;
            varPanel.ResumeLayout();
        }


        public void ProcessSelectedNodeTEV()
        {
            TreeNode node = nodesTree.SelectedNode;

            Be.Windows.Forms.DynamicByteProvider provider = null;

            if (node is TreeNodeSet1)
            {
                TreeNodeSet1 set1Node = (node as TreeNodeSet1);

                provider = new Be.Windows.Forms.DynamicByteProvider(set1Node.Set.Unk1 != null ? set1Node.Set.Unk1 : new byte[0]);
                provider.Changed += delegate
                {
                    if (provider.Bytes.Count == set1Node.Set.Unk1.Length)
                        set1Node.Set.Unk1 = provider.Bytes.ToArray();
                };
                Set1Window.Draw(this, set1Node.Set);


                switch (set1Node.Set.Type)
                {
                    case ObjectNodeCategory.HumanOrWeapon:
                        if (set1Node.Set is ObjectHuman)
                            ObjectHumanWindow.Draw(this, set1Node.Set as ObjectHuman);
                        break;
                }

            }
            else if (node is TreeNodeSet2)
            {
                TreeNodeSet2 set2Node = (node as TreeNodeSet2);

                provider = new Be.Windows.Forms.DynamicByteProvider(set2Node.Set.Unk2);
                provider.Changed += delegate
                {
                    if (provider.Bytes.Count == set2Node.Set.Unk2.Length)
                        set2Node.Set.Unk2 = provider.Bytes.ToArray();
                };
                Set2Window.Draw(this, set2Node.Set);

                if (set2Node.Set.Type == Set2NodeCategory.Element)
                {
                    Set2Element elem = set2Node.Set as Set2Element;

                    if (elem is Set2Element1019)
                        DrawElement1019(elem as Set2Element1019);
                    else if (elem.Effect != null)
                        DrawTEVEffectWindow(elem.Effect);

                }
            }
            else if (node is TreeNodeEffect)
            {
                DrawTEVEffectWindow((node as TreeNodeEffect).Effect);
                varPanel.ResumeLayout();
                return;
            }


            unkBytesBox.ByteProvider = provider;
            varPanel.ResumeLayout();
        }

        private void DrawElement1019(Set2Element1019 element, bool isCSVTree = false)
        {
            Set2Element1019Window.Draw(this, element);
            DrawHActEvent(TevCsvEntry.TryGetHActEventData(element.Type1019), isCSVTree);
        }


        private void DrawHActEvent(CSVHActEvent hevent, bool isCSVTree)
        {
            if (hevent == null)
                return;

            CreateHeader("HAct Event (CSV)", isCsvTree: isCSVTree);
            CreateInput("Type", hevent.Type, delegate (string val) { hevent.Type = val; }, isCsvTree: isCSVTree);

            CreateInput("Unknown", hevent.HEUnknown1.ToString(), delegate (string val) { hevent.HEUnknown1 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown2.ToString(), delegate (string val) { hevent.HEUnknown2 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown3.ToString(), delegate (string val) { hevent.HEUnknown3 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown4.ToString(), delegate (string val) { hevent.HEUnknown4 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown5.ToString(), delegate (string val) { hevent.HEUnknown5 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown6.ToString(), delegate (string val) { hevent.HEUnknown6 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);
            CreateInput("Unknown", hevent.HEUnknown7.ToString(), delegate (string val) { hevent.HEUnknown7 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCSVTree);

            if (string.IsNullOrEmpty(hevent.Type))
                return;

            string[] split = hevent.Type.Split('_');

            if (split.Length < 3)
                return;

            switch (split[1])
            {
                case "GAUGE":
                    HActEventGaugeWindow.Draw(this, (CSVHActEventHeatGauge)hevent, isCSVTree);
                    break;
                case "DAMAGE":
                    if (split[2] != "99")
                        HActEventDamageWindow.Draw(this, (CSVHActEventDamage)hevent, isCSVTree);
                    break;
            }
        }


        //Example: expression target
        private void ProcessSpecialSelectedNode()
        {
            if (nodesTree.SelectedNode == null)
                return;

            switch (nodesTree.SelectedNode.GetType().Name)
            {
                case "TreeViewItemExpressionTargetDataOE":
                    TreeViewItemExpressionTargetDataOE itemExpDatOE = nodesTree.SelectedNode as TreeViewItemExpressionTargetDataOE;

                    CreateButton("Curve", delegate
                    {
                        CurveView myNewForm = new CurveView();
                        myNewForm.Visible = true;
                        myNewForm.Init(itemExpDatOE.Data.AnimationData,
                            delegate (byte[] outCurve)
                            {
                                itemExpDatOE.Data.AnimationData = outCurve;
                            });
                    });

                    break;

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
            if (nodesTree.SelNodes.Count == 1)
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


        private void UnselectSelectedNodes()
        {
            MWTreeNodeWrapper[] nodes = nodesTree.SelNodes.Values.Cast<MWTreeNodeWrapper>().ToArray();
            Hashtable table = (Hashtable)nodesTree.SelNodes.Clone();

            foreach (DictionaryEntry kv in table)
            {
                MWTreeNodeWrapper node = (MWTreeNodeWrapper)kv.Value;
                node.Deselect();
                nodesTree.DeselectNode(node.Node, true);
            }

            nodesTree.SelNodes.Clear();
        }

        private void DeleteSelectedNodes()
        {
            MWTreeNodeWrapper[] nodes = nodesTree.SelNodes.Values.Cast<MWTreeNodeWrapper>().ToArray();
            Hashtable table = (Hashtable)nodesTree.SelNodes.Clone();

            foreach (DictionaryEntry kv in table)
            {
                MWTreeNodeWrapper node = (MWTreeNodeWrapper)kv.Value;
                node.Deselect();
                nodesTree.DeselectNode(node.Node, true);
            }

            nodesTree.SelNodes.Clear();

            foreach (MWTreeNodeWrapper node in nodes)
                nodesTree.RemoveNode(node);
        }

        private void PasteNode(TreeNode[] pastingNode)
        {
            if (pastingNode == null || pastingNode.Length == 0)
                return;

            if(IsPS2Prop)
            {
                foreach(var node in pastingNode)
                {
                    TreeNode newNode = (TreeNode)node.Clone();
                    nodesTree.Nodes.Add(newNode);
                }
                return;
            }

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
                        newNode.HActNode.Guid = hactNode.HActNode.BEPDat.Guid2;
                    }
                }

                if (IsTev)
                {
                    TreeNodeSet1 obj = nodesTree.SelectedNode as TreeNodeSet1;

                    if (obj == null)
                        return;

                    foreach (TreeNode node in pastingNode)
                    {
                        TreeNode newNode = (TreeNode)node.Clone();
                        obj.Nodes.Add(newNode);
                    }

                    UnselectSelectedNodes();
                    return;
                }

                if (pastingNode.Length > 0)
                {
                    if (pastingNode[0] is TreeViewItemMepNode)
                    {
                        //so bad
                        foreach (TreeViewItemMepNode node in pastingNode)
                        {
                            if (node.Node is MepEffectOE)
                            {
                                NodeElement nodeElem = (node.Node as MepEffectOE).Effect;
                                nodeElem.Name = TreeViewItemNode.TranslateName(nodeElem);
                                PasteNode(new TreeViewItemNode(nodeElem));
                            }
                        }
                    }
                }

                foreach (var node in pastingNode.Where(x => x as TreeViewItemNode != null))
                {
                    PasteNode((node as TreeViewItemNode));
                }

                UnselectSelectedNodes();
            }
            else
            {
                if (IsMep)
                {
                    foreach (TreeNode node in pastingNode)
                    {
                        //Mep to mep paste
                        if (node is TreeViewItemMepNode)
                        {
                            TreeViewItemMepNode orig = node as TreeViewItemMepNode;
                            MepEffect cloned = orig.Node.Copy();
                            TreeViewItemMepNode newNode = new TreeViewItemMepNode(cloned);

                            nodesTree.Nodes.Add(newNode);
                        } //Mep to hact paste
                        else
                        {
                            Node hactNode = (node as TreeViewItemNode).HActNode;

                            if (hactNode.Category == AuthNodeCategory.Element)
                            {

                                MepEffectOE effectMep = new MepEffectOE();
                                effectMep.Effect = (NodeElement)hactNode.Copy();

                                nodesTree.Nodes.Add(new TreeViewItemMepNode(effectMep));
                            }
                        }

                        UnselectSelectedNodes();
                    }
                }
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
        public void GenerateBaseInfo(BaseCMN cmn)
        {
            cmn.Version = OECMN.GetCMNVersionForGame(curGame);
            cmn.HActStart = Utils.InvariantParse(hactStartBox.Text);
            cmn.HActEnd = Utils.InvariantParse(hactEndBox.Text);
            cmn.CutInfo = CutInfos.OrderBy(x => x).ToArray();
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

        public CMN GenerateHAct()
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

        public OECMN GenerateOEHAct()
        {
            OECMN cmn = new OECMN();
            cmn.SoundInfo = SoundInfoOE;

            return cmn;
        }

        public BEP GenerateBep()
        {
            BEP bep = new BEP();
            bep.Game = curGame;
            bep.Nodes.AddRange(GetAllNodes());


            return bep;
        }

        public string GetLocalizedCMN()
        {
            string lang = langOverrideBox.Text;

            if (string.IsNullOrEmpty(lang))
                return "cmn.bin";
            else
                return $"cmn_{lang}.bin";
        }

        public string GetLocalizedRES()
        {
            string lang = langOverrideBox.Text;

            if (string.IsNullOrEmpty(lang))
                return "res.bin";
            else
                return $"res_{lang}.bin";
        }

        private ObjectBase GenerateTEVHierarchy()
        {
            void ChildLoop(TreeNode node)
            {
                if (node as TreeNodeSet1 == null)
                    return;

                TreeNodeSet1 set1Node = node as TreeNodeSet1;
                set1Node.Set.Children.Clear();

                foreach (TreeNode treeChild in node.Nodes.Cast<TreeNode>())
                {
                    if (treeChild is TreeNodeSet1)
                    {
                        TreeNodeSet1 set1Child = (treeChild as TreeNodeSet1);

                        set1Node.Set.Children.Add(set1Child.Set);
                        set1Child.Set.Parent = set1Node.Set;
                    }
                    else if (treeChild is TreeNodeSet2)
                        set1Node.Set.Children.Add((treeChild as TreeNodeSet2).Set);
                    else if (treeChild is TreeNodeEffect)
                        set1Node.Set.Children.Add((treeChild as TreeNodeEffect).Effect);

                    ChildLoop(treeChild);
                }
            }

            ChildLoop(nodesTree.Nodes[0]);

            return (nodesTree.Nodes[0] as TreeNodeSet1).Set;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yarhl.IO.DataStream stream = null;

            if (IsBep || IsMep)
                if (string.IsNullOrEmpty(FilePath))
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.DefaultExt = (IsBep ? ".bep" : ".mep");
                    dialog.ShowDialog();
                    FilePath = dialog.FileName;
                }

            CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;
            curGame = (Game)targetGameCombo.SelectedIndex;

            //Write hact
            if (IsHact)
            {
                if (!hactInf.IsPar)
                {
                    BaseCMN cmn = (IsOE ? GenerateOEHAct() : GenerateHAct());
                    GenerateBaseInfo(cmn);
                    if (IsOE)
                        OECMN.Write(cmn as OECMN, Path.Combine(folderDir, $"cmn.bin"));
                    else
                        CMN.Write(cmn as CMN, FilePath);


                    if (hactInf.GetResources().Length > 0)
                    {
                        RES newRes = new RES();

                        foreach (TreeViewItemResource res in resTree.Nodes)
                            newRes.Resources.Add(res.Resource);

                        RES.Write(newRes, hactInf.GetResources()[0].FindFile(GetLocalizedRES()).Path, CMN.IsDE(curVer));
                    }
                }
            }
            else if (IsBep)
                BEP.Write(GenerateBep(), FilePath, curGame);
            else if (IsMep)
            {
                curGame = (Game)targetGameCombo.SelectedIndex;
                ConvertCurrentMep();
                MEP.Write(Mep, FilePath);
            }
            else if (IsTev)
            {
                Tev.Root = GenerateTEVHierarchy();
                TEV.Write(Tev, FilePath);

                string csvPath = null;

                if (Csv != null)
                {
                    if (curGame == Game.Y3)
                        csvPath = INISettings.Y3CsvPath;
                    else if (curGame == Game.Y4)
                        csvPath = INISettings.Y4CsvPath;

                    if (MessageBox.Show("Save CSV too?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        CSV.Write(Csv, csvPath);
                }
            }
            else if (IsPS2Prop)
            {
                OMTProperty propFile = new OMTProperty();

                List<OMTMoveProperty> dat1s = nodesTree.Nodes.Cast<TreeNode>().Where(x => x is TreeNodePS2PropertyData1).Cast<TreeNodePS2PropertyData1>().Select(x => x.Data1).ToList();
                List<OMTEffectProperty> dat2s = nodesTree.Nodes.Cast<TreeNode>().Where(x => x is TreeNodePS2PropertyData2).Cast<TreeNodePS2PropertyData2>().Select(x => x.Data2).ToList();

                propFile.MoveProperties = dat1s;
                propFile.Effects = dat2s;

                OMTProperty.Write(propFile, FilePath);
            }
            else if(IsYAct)
            {
                YActY1.Write(FilePath, yact);
            }

        }


        private void ConvertCurrentMep()
        {
            curGame = (Game)targetGameCombo.SelectedIndex;
            MepEffect[] mepNodes = nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).Cast<MepEffect>().ToArray();

            if (Mep.Version == MEPVersion.Y5 || Mep.Version == MEPVersion.Y0)
            {

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
            }

            Mep.Effects = nodesTree.Nodes.Cast<TreeViewItemMepNode>().Select(x => x.Node).ToList();

            switch (curGame)
            {
                default:
                    Mep.Version = MEPVersion.Y0;
                    break;
                case Game.Y3:
                    Mep.Version = MEPVersion.Y3;
                    break;
                case Game.Y4:
                    Mep.Version = MEPVersion.Y3;
                    break;
                case Game.Y5:
                    Mep.Version = MEPVersion.Y5;
                    break;
                case Game.Y0:
                    Mep.Version = MEPVersion.Y0;
                    break;
                case Game.YK1:
                    Mep.Version = MEPVersion.Y0;
                    break;
            }
        }


        //ELEMENT WINDOW DRAWING

        public void DrawElementWindow(NodeElement element)
        {
            DrawWindow.Draw(element);
        }

        public NodeElement[] GetCurrentAuthAllElements()
        {
            Node[] allNodes = GetAllNodes();
            return allNodes.Where(x => x.Category == AuthNodeCategory.Element).Cast<NodeElement>().ToArray();
        }

        public Node[] GetAllNodes()
        {

            if (!IsMep)
            {
                List<TreeNode> nodes = new List<TreeNode>();

                void Process(TreeNode node)
                {
                    nodes.Add(node);

                    foreach (TreeNode child in node.Nodes)
                        Process(child);
                }

                if (!IsBep)
                    Process(nodesTree.Nodes[0]);
                else
                {
                    foreach (TreeNode node in nodesTree.Nodes)
                        Process(node);
                }

                return nodes.Where(x => x is TreeViewItemNode)
                   .Cast<TreeViewItemNode>()
                   .Select(x => x.HActNode)
                   .ToArray();
            }
            else
            {
                List<Node> children = new List<Node>();

                void ChildLoop(TreeNode parent)
                {
                    foreach (TreeNode node in parent.Nodes)
                    {
                        TreeViewItemNode hactNode = node as TreeViewItemNode;

                        if (hactNode != null)
                        {
                            hactNode.Nodes.Add(hactNode);
                            ChildLoop(hactNode);
                        }
                    }
                }

                TreeViewItemNode[] rootNodes = GetAllNodesTreeView();


                return nodesTree.Nodes
                   .Cast<TreeViewItemNode>()
                   .Select(x => x.HActNode)
                   .ToArray();
            }
        }

        public TreeNode[] GetAllTreeNodes()
        {

            if (!IsMep)
            {
                List<TreeNode> nodes = new List<TreeNode>();

                void Process(TreeNode node)
                {
                    nodes.Add(node);

                    foreach (TreeNode child in node.Nodes)
                        Process(child);
                }

                if (!IsBep)
                    Process(nodesTree.Nodes[0]);
                else
                {
                    foreach (TreeNode node in nodesTree.Nodes)
                        Process(node);
                }

                return nodes.ToArray();
            }
            else
            {
                List<TreeNode> children = new List<TreeNode>();

                void ChildLoop(TreeNode parent)
                {
                    foreach (TreeNode node in parent.Nodes)
                    {
                        children.Add(node);
                        ChildLoop(node);
                    }
                }

                foreach (TreeNode node in nodesTree.Nodes)
                {
                    children.Add(node);
                    ChildLoop(node);
                }


                return children.ToArray();
            }
        }

        private TreeViewItemNode[] GetAllNodesTreeView()
        {
            return nodesTree.Nodes.Cast<TreeNode>()
               .Where(x => x is TreeViewItemNode)
               .Cast<TreeViewItemNode>()
               .ToArray();
        }

        private List<TreeViewItemNode> GetAllHActNodesRecursive(TreeViewItemNode nodeStart)
        {
            List<TreeViewItemNode> list = new List<TreeViewItemNode>();
            list.Add(nodeStart);

            foreach (TreeNode node in nodeStart.Nodes)
            {
                TreeViewItemNode node2 = node as TreeViewItemNode;

                if (node2 != null)
                {
                    list.AddRange(GetAllHActNodesRecursive(node2));
                }

            }

            return list;
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
            EditingResource.Resource.StartFrame = Utils.InvariantParse(resStartBox.Text);
            EditingResource.Resource.EndFrame = Utils.InvariantParse(resEndBox.Text);
        }

        private void resTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EditingResource = e.Node as TreeViewItemResource;
            resourceNameTextbox.Text = EditingResource.Resource.Name;
            resourceTypeBox.SelectedIndex = (int)EditingResource.Resource.Type;
            resStartBox.Text = EditingResource.Resource.StartFrame.ToString(CultureInfo.InvariantCulture);
            resEndBox.Text = EditingResource.Resource.EndFrame.ToString(CultureInfo.InvariantCulture);

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

            CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;

            //TODO: GENERATE HACT
            if (IsHact)
            {
                BaseCMN cmn = (IsOE ? GenerateOEHAct() : GenerateHAct());
                GenerateBaseInfo(cmn);
                if (IsOE)
                    OECMN.Write(cmn as OECMN, dialog.FileName);
                else
                    CMN.Write(cmn as CMN, dialog.FileName);
            }
            else
            {
                if (IsBep)
                    BEP.Write(GenerateBep(), dialog.FileName, curVer);
                else if (IsMep)
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

            EditingNode = null;
            EditingResource = null;
            m_rightClickedNode = null;

            curGame = (Game)targetGameCombo.SelectedIndex;
            curVer = CMN.GetVersionForGame(curGame);
            CMN.LastHActDEGame = (Game)targetGameCombo.SelectedIndex;
            FilePath = dialog.FileName;

            nodesTree.SuspendLayout();

            if(curGame == Game.Y1 || curGame == Game.Y2)
            {
                FilePath = dialog.FileName;
                OMTProperty property = OMTProperty.Read(dialog.FileName);

                ProcessPS2Prop(property);
            }
            else if(curGame > Game.Y3 && curGame < Game.Y6Demo)
            {
                Mep = MEP.Read(dialog.FileName);
                InitMEP(Mep);
            }
            else
            {
                BEP Bep = BEP.Read(dialog.FileName, curGame);
                FilePath = dialog.FileName;

                InitBEP(Bep);
            }

            nodesTree.ResumeLayout();
        }


        private void OnOpenDEFormat()
        {
            addNodeTab.DropDownItems.Add(nodeAddTabDE);
            advancedTab.DropDownItems.Add(authPagesButton);
            convertTab.DropDownItems.Add(convertBetweenGamesButton);
        }
        private void OnOpenHAct()
        {
            advancedTab.DropDownItems.Add(advancedFrameProgressionButton);
            advancedTab.DropDownItems.Add(disableFrameInfoButton);
        }

        private void InitBEP(BEP Bep)
        {
            ClearEverything();

            SetBEPMode();

            foreach (Node node in Bep.Nodes)
                nodesTree.Nodes.Add(new TreeViewItemNode(node));

            ProcessBEPHierarchy();
        }

        private void InitMEP(MEP Mep)
        {
            ClearEverything();

            EditingNode = null;
            EditingResource = null;
            m_rightClickedNode = null;

            IsHact = false;
            IsOE = true;
            IsMep = true;
            IsBep = false;
            IsTev = false;

            hactTabs.TabPages[0].Text = "MEP";
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            hactTabs.TabPages[0].Text = "MEP";
            hactTabs.TabPages.Remove(resPage);
            hactTabs.TabPages.Remove(cutPage);

            resTree.Nodes.Clear();

            foreach (MepEffect node in Mep.Effects)
                nodesTree.Nodes.Add(new TreeViewItemMepNode(node));
        }

        public void ProcessBEPHierarchy()
        {
            TreeViewItemNode[] nodes = GetAllNodesTreeView();

            foreach (TreeViewItemNode node in nodes)
            {
                TreeViewItemNode parentNode = nodes.Where(x => x.HActNode.BEPDat.Guid2 == node.HActNode.Guid).FirstOrDefault();

                if (parentNode != null && parentNode != node)
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
            OnOpenDEFormat();
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


        private void transitStunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeElement stun = AddElementOfType(typeof(DEElementTimingInfoStun), "Stun Transit", "e_auth_element_battle_transit_stun");
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
                m_rightClickedNode = (TreeNode)nodesTree.GetNodeAt(e.X, e.Y);
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

            PibVersion targetVer = PibVersion.Y5;

            switch (curGame)
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

            for (int i = 0; i < mepPibs.Count; i++)
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
                switch (game)
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

            foreach (MepEffectOE mepOe in mep.Effects.Cast<MepEffectOE>())
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

            if (MessageBox.Show("Convert particle files?", "Conversion", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

                foreach (BasePib pib in mepPibs)
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

                foreach (DEElementParticle ptc in GetAllNodesTreeView().Where(x => x.HActNode is DEElementParticle).Select(x => x.HActNode).Cast<DEElementParticle>())
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
            nodesTree.SuspendLayout();

            Form1.TranslateNames = checkBox1.Checked;


            foreach (TreeViewItemNode rootNode in nodesTree.Nodes)
            {
                foreach (TreeViewItemNode node in GetAllHActNodesRecursive(rootNode))
                {
                    if (Form1.TranslateNames)
                        node.Text = TreeViewItemNode.TranslateName(node.HActNode);
                    else
                        node.Text = node.HActNode.Name;
                }
            }

            nodesTree.ResumeLayout();
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

            FrameProgressionWindow curveForm = new FrameProgressionWindow();
            curveForm.Visible = true;
        }

        private void authPagesDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pager pager = new Pager();
            pager.Visible = true;
        }

        private void disableFrameInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableFrameWindow pager = new DisableFrameWindow();
            pager.Visible = true;
            pager.Init(DisableFrameInfos);
        }

        //Ryuse Ga Gotoku
        private void convertBetweenGamesDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curVer < GameVersion.Y0_K1)
                return;

            RyuseWindow wind = new RyuseWindow();
            wind.Visible = true;

            return;

            List<string> options = new List<string>();

            if (!IsOE)
            {
                Game[] deGames = CMN.GetDEGames();

                foreach (Game game in deGames)
                    options.Add(HActLib.Internal.Reflection.GetGamePrefixes(game)[0]);
            }
            else
            {
                Game[] oeGames = CMN.GetOEGames();

                foreach (Game game in oeGames)
                    options.Add(HActLib.Internal.Reflection.GetGamePrefixes(game)[0]);
            }

            string messageString = "Please enter the name of game you want to convert to. \nAvailable:\n";

            foreach (string str in options)
                messageString += str + "\n";

            string input = Microsoft.VisualBasic.Interaction.InputBox(messageString,
                            "Convert",
                            "",
                            0,
                            0);

            Game prefixGame = CMN.GetGameFromString(input);

            if ((uint)prefixGame >= 9999)
                return;

            Node[] nodes = null;

            GameVersion targetVer = CMN.GetVersionForGame(prefixGame);

            if (IsBep || IsMep)
                nodes = GetAllNodes();
            else
            {
                BaseCMN genHact = null;

                if (IsOE)
                    genHact = GenerateOEHAct();
                else
                    genHact = GenerateHAct();

                GenerateBaseInfo(genHact);
                nodes = genHact.GetNodes();


                for (int i = 0; i < AuthPagesDE.Length; i++)
                {
                    AuthPage page = AuthPagesDE[i];

                    //Convert between old and new formats
                    if ((page.Format == 0 && targetVer > GameVersion.Yakuza6) || (page.Format > 0 && targetVer <= GameVersion.Yakuza6))
                    {
                        page.PageIndex = i;

                        foreach (Transition transition in page.Transitions)
                            foreach (Condition condition in transition.Conditions)
                            {
                                string name = ConditionConvert.GetName(condition.ConditionID, curGame);
                                uint newID = ConditionConvert.GetID(name, prefixGame);
                                condition.ConditionID = newID;
                            }
                    }

                    if (targetVer <= GameVersion.Yakuza6)
                        page.Format = 0;
                    else if (targetVer == GameVersion.DE1)
                        page.Format = 1;
                    else
                        page.Format = 2;

                    if (page.IsTalkPage())
                    {
                        if (curVer <= GameVersion.DE1 && targetVer >= GameVersion.DE2)
                        {
                            if ((page.Flag & 0x40) == 0)
                                page.Flag |= 0x40;
                        }
                    }
                    else
                    {
                        if (curVer <= GameVersion.DE1)
                            page.Flag = 0;
                    }
                }

            }

            Node[] outputNodes;

            if (!IsOE)
            {
                RyuseModule.ConversionInformation inf = RyuseModule.ConvertNodes(nodes, Form1.curGame, prefixGame);
                outputNodes = inf.OutputNodes;
            }
            else
            {
                RyuseOEModule.ConversionInformation inf = RyuseOEModule.ConvertNodes(nodes, Form1.curGame, prefixGame);
                outputNodes = inf.OutputNodes;
            }

            curGame = prefixGame;
            curVer = CMN.GetVersionForGame(curGame);
            CMN.LastHActDEGame = curGame;

            targetGameCombo.SelectedIndex = (int)curGame;

            ClearNodes();

            if (!IsBep)
            {
                nodesTree.Nodes.Add(new TreeViewItemNode(outputNodes[0]));
            }
            else
            {
                foreach (Node node in outputNodes)
                    nodesTree.Nodes.Add(new TreeViewItemNode(node));

                ProcessBEPHierarchy();
            }
        }

        private void propertybinTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CMN.IsDEGame((Game)targetGameCombo.SelectedIndex))
            {
                MessageBox.Show("Select a DE game first before converting!", "Choose DE Game", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                return;
            }

            OpenFileDialog propertyBinPath = new OpenFileDialog();

            if (propertyBinPath.ShowDialog() != DialogResult.OK)
                return;

            OldEngineFormat oeProperty = OldEngineFormat.Read(propertyBinPath.FileName);

            string input = Microsoft.VisualBasic.Interaction.InputBox("Entry name to convert. Leave blank to convert everything",
                "Property to BEP",
                "",
                0,
                0);

            bool convertAll = string.IsNullOrEmpty(input);

            if (convertAll)
            {
                MessageBox.Show("Choose which directory to save all the converted properties to");

                FolderBrowserDialog folderDialog = new FolderBrowserDialog();

                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                foreach (OEAnimEntry entry in oeProperty.Moves)
                {
                    BEP converted = ConvertAnimEntryToBEP(entry);
                    BEP.Write(converted, Path.Combine(folderDialog.SelectedPath, entry.Name + ".bep"), CMN.GetVersionForGame((Game)targetGameCombo.SelectedIndex));
                }
            }
            else
            {
                OEAnimEntry entry = oeProperty.Moves.Where(x => x.Name == input).FirstOrDefault();

                if (entry == null)
                {
                    MessageBox.Show($"Couldn't find {input}, please check case sensitivity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                BEP converted = ConvertAnimEntryToBEP(entry);
                InitBEP(converted);
            }
        }

        private BEP ConvertPropertyBinToBEP(OldEngineFormat format)
        {
            BEP bep = new BEP();

            return bep;
        }

        private static BEP ConvertAnimEntryToBEP(OEAnimEntry entry)
        {
            BEP bep = new BEP();

            foreach (OEAnimProperty property in entry.Properties)
            {
                Node convertedNode = ConvertPropertyToNode(property, 16, Game.YK1, curGame);

                if (convertedNode != null)
                    bep.Nodes.Add(convertedNode);
            }

            return bep;
        }

        private static Node ConvertPropertyToNode(OEAnimProperty property, uint version, Game oeGame, Game game)
        {
            NodeElement deNode = null;

            switch (property.Type)
            {
                case OEPropertyType.VoiceAudio:
                    ushort voiceCuesheet = 0;

                    voiceCuesheet = RyuseModule.GetGVFighterIDForGame(game);

                    OEAnimPropertyVoiceSE oePropertyVoiceSE = property as OEAnimPropertyVoiceSE;
                    DEElementSE voiceSE = new DEElementSE();

                    voiceSE.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
                    voiceSE.CueSheet = voiceCuesheet;
                    voiceSE.SoundIndex = (byte)oePropertyVoiceSE.ID;
                    voiceSE.Unk = 128;

                    deNode = voiceSE;
                    break;

                case OEPropertyType.SEAudio:
                    OEAnimPropertySE oePropertySE = property as OEAnimPropertySE;
                    DEElementSE seNode = new DEElementSE();

                    seNode.ElementKind = Reflection.GetElementIDByName("e_auth_element_se", game);
                    seNode.CueSheet = oePropertySE.Cuesheet;
                    seNode.SoundIndex = (byte)(oePropertySE.ID + 1);

                    deNode = seNode;
                    break;

                case OEPropertyType.FollowupWindow:
                    DEElementFollowupWindow deFollowup = new DEElementFollowupWindow();
                    deFollowup.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_followup_window", game);

                    deNode = deFollowup;
                    break;

                case OEPropertyType.ControlLock:
                    DEElementControlLock deControl = new DEElementControlLock();
                    deControl.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_control_window", game);

                    deNode = deControl;
                    break;

                case OEPropertyType.Hitbox:
                    OEAnimPropertyHitbox oeHitbox = property as OEAnimPropertyHitbox;
                    DETimingInfoAttack attack = new DETimingInfoAttack();

                    attack.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_attack", game);

                    attack.Data.Damage = oeHitbox.Damage;

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftHand))
                        attack.Data.Parts |= 4104;

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightHand))
                        attack.Data.Parts |= 2052;

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftElbow))
                        attack.Data.Parts |= 5128;

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.RightElbow))
                        attack.Data.Parts |= 2564;

                    if (oeHitbox.HitboxLocation1.HasFlag(HitboxLocation1Flag.LeftKnee))
                        attack.Data.Parts |= 81952;

                    //Placeholder
                    attack.Data.Attributes = 1;
                    attack.Data.Power = 1;


                    deNode = attack;

                    break;

                case OEPropertyType.Muteki:
                    DETimingInfoMuteki invincibility = new DETimingInfoMuteki();
                    invincibility.ElementKind = Reflection.GetElementIDByName("e_auth_element_battle_muteki", game);

                    deNode = invincibility;

                    break;

            }

            if (deNode != null)
            {
                deNode.Start = property.Start;
                deNode.End = property.End;
                deNode.BEPDat.Guid2 = Guid.NewGuid();
                deNode.PlayType = ElementPlayType.Normal;
                deNode.UpdateTimingMode = 1;
            }

            return deNode;
        }

        private void ımportBEPDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            BEP bep = BEP.Read(dialog.FileName, (Game)targetGameCombo.SelectedIndex);


            foreach (Node node in bep.Nodes)
                nodesTree.Nodes.Add(new TreeViewItemNode(node));
        }

        private TreeNode DrawObject(ITEVObject obj)
        {
            if (obj is ObjectBase)
                return DrawTEVSet1(obj as ObjectBase);
            else if (obj is Set2)
                return DrawTEVSet2(obj as Set2);
            else
                return new TreeNodeEffect(obj as EffectBase);
        }

        private TreeNodeSet1 DrawTEVSet1(ObjectBase set)
        {
            TreeNodeSet1 node = new TreeNodeSet1(set);

            foreach (ITEVObject child in set.Children)
                node.Nodes.Add(DrawObject(child));

            return node;
        }

        private TreeNode DrawTEVSet2(Set2 set)
        {
            TreeNodeSet2 node = new TreeNodeSet2(set);

            return node;
        }

        private TreeNode DrawTEVEffect(EffectBase effect)
        {
            TreeNodeEffect node = new TreeNodeEffect(effect);

            return node;
        }

        private void DrawTEVEffectWindow(EffectBase effect)
        {
            EffectWindowBase.Draw(this, effect);
            Be.Windows.Forms.DynamicByteProvider provider = new Be.Windows.Forms.DynamicByteProvider(effect.Data == null ? new byte[0] : effect.Data);
            provider.Changed += delegate { effect.Data = provider.Bytes.ToArray(); };

            if ((effect as EffectElement) != null)
                EffectElementWindow.Draw(this, effect as EffectElement);

            switch (effect.ElementKind)
            {
                case EffectID.Sound:
                    EffectWindowSound.Draw(this, effect as EffectSound);
                    break;
                case EffectID.Particle:
                    EffectWindowParticle.Draw(this, effect as EffectParticle);
                    break;
            }

            unkBytesBox.ByteProvider = provider;
        }


        TreeNode m_csvCharactersRoot;
        TreeNode m_csvHActEventsRoot;
        private void DrawCSV()
        {
            UpdateCSV();

            csvTree.Nodes.Clear();

            m_csvCharactersRoot = new TreeNode("Characters");
            m_csvHActEventsRoot = new TreeNode("HAct Events");

            m_csvCharactersRoot.ContextMenuStrip = csvContextHEvent;
            m_csvHActEventsRoot.ContextMenuStrip = csvContextHEvent;

            csvTree.Nodes.Add(m_csvCharactersRoot);
            csvTree.Nodes.Add(m_csvHActEventsRoot);

            foreach (CSVCharacter chara in TevCsvEntry.Characters)
            {
                m_csvCharactersRoot.Nodes.Add(new TreeNodeCSVCharacter(chara));
            }

            foreach (CSVHActEvent hevent in TevCsvEntry.SpecialNodes)
            {
                m_csvHActEventsRoot.Nodes.Add(new TreeNodeCSVHActEvent(hevent));
            }
        }


        private TreeNode m_selectedNodeCsvTree;
        private void csvTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            csvVarPanel.Controls.Clear();
            csvVarPanel.RowCount = 0;
            csvVarPanel.RowStyles.Clear();

            TreeNode selectedNode = e.Node;

            if (selectedNode is TreeNodeCSVCharacter)
            {
                CSVCharacterWindow.Draw(this, (selectedNode as TreeNodeCSVCharacter).Character, true);
            }
            else if (selectedNode is TreeNodeCSVHActEvent)
            {
                CSVHActEvent hevent = (selectedNode as TreeNodeCSVHActEvent).Event;
                DrawHActEvent(hevent, true);
            }
        }

        private TreeNodeSet2[] GetAllHActEvents()
        {
            return GetAllTreeNodes().Where(x => x is TreeNodeSet2).Cast<TreeNodeSet2>().Where(x => x.Set is Set2Element1019).ToArray();
        }

        //Example:
        //HE_DAMAGE_00
        //HE_DAMAGE_01
        //Returns: 1
        private int GetCurrentIDForEvent(string type)
        {
            if (Csv == null || TevCsvEntry == null)
                return 0;

            return TevCsvEntry.SpecialNodes.Where(x => x.Type.StartsWith(type)).Count();
        }

        private void UpdateCSV()
        {
            if (Csv == null || TevCsvEntry == null)
                return;

            //Order csv events
            TreeNodeSet2[] events = GetAllHActEvents().GroupBy(x => (x.Set as Set2Element1019).Type1019).Select(y => y.First()).ToArray();

            List<CSVHActEvent> eventsCsv = new List<CSVHActEvent>();

            foreach (TreeNodeSet2 set2 in events)
            {
                Set2Element1019 elem = set2.Set as Set2Element1019;
                eventsCsv.Add(TevCsvEntry.TryGetHActEventData(elem.Type1019));
            }

            TevCsvEntry.SpecialNodes = eventsCsv.ToList();
        }

        private void csvTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                m_selectedNodeCsvTree = (TreeNode)csvTree.GetNodeAt(e.X, e.Y);
        }

        private void addCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSVCharacter chara = new CSVCharacter();
            chara.UnknownExtraData.Add(new CSVCharacterExtraData());
            TevCsvEntry.Characters.Add(chara);

            m_csvCharactersRoot.Nodes.Add(new TreeNodeCSVCharacter(chara));
        }

        private void addDamageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSVHActEventDamage dmg = new CSVHActEventDamage();
            TevCsvEntry.SpecialNodes.Add(dmg);

            m_csvHActEventsRoot.Nodes.Add(new TreeNodeCSVHActEvent(dmg));
        }

        private void addHeatChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSVHActEventHeatGauge gauge = new CSVHActEventHeatGauge();
            TevCsvEntry.SpecialNodes.Add(gauge);

            m_csvHActEventsRoot.Nodes.Add(new TreeNodeCSVHActEvent(gauge));
        }

        private void damageHActEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Csv == null || TevCsvEntry == null)
                return;

            TreeNode parent = nodesTree.SelectedNode == null ? nodesTree.Nodes[0] : nodesTree.SelectedNode;

            Set2Element1019 elem = new Set2Element1019();
            elem.Type1019 = "HE_DAMAGE_" + (TevCsvEntry.SpecialNodes.Where(x => x.Type.StartsWith("HE_DAMAGE_") && !x.Type.EndsWith("99")).Count()).ToString("D2");

            CSVHActEventDamage dmg = new CSVHActEventDamage();
            dmg.Type = elem.Type1019;

            TevCsvEntry.SpecialNodes.Add(dmg);
            parent.Nodes.Add(new TreeNodeSet2(elem));
        }

        public void ProcessSelectedNodeYAct()
        {
            TreeNode node = nodesTree.SelectedNode;

            Be.Windows.Forms.DynamicByteProvider provider = null;

            if(node is TreeNodeYActY2Animation)
            {
                var y2Anim = node as TreeNodeYActY2Animation;
                CreateHeader($"Animation ({y2Anim.Animation.Format})");
                CreateInput("Start", y2Anim.Animation.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { y2Anim.Animation.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                CreateInput("End", y2Anim.Animation.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { y2Anim.Animation.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                CreateButton("Export", delegate
                {
                    string format = y2Anim.Animation.Format.ToString().ToLowerInvariant();

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = $"PS2 Animation File (*.{format})|*.{format}";
                    dialog.FileName = $"{y2Anim.Parent.Text}.{format}";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, y2Anim.Animation.File.Buffer);
                    }
                });
            }

            if (node is TreeNodeYActCameraMotion)
            {
                CreateHeader("Camera Animation");
                CreateButton("Import", delegate
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "PS2 Object/Camera Animation (*.mtbw)|*.mtbw";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var animNode = (node as TreeNodeYActCameraMotion);
                        animNode.File.Buffer = File.ReadAllBytes(dialog.FileName);

                        if (animNode.OrigCamera != null)
                            animNode.OrigCamera.MTBWFile = animNode.File.Buffer;
                    }
                });
                CreateButton("Export", delegate
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "PS2 Camera Animation (*.mtbw)|*.mtbw";
                    dialog.FileName = "camera.mtbw";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, (node as TreeNodeYActCameraMotion).File.Buffer);
                    }
                });
            }
            else if (node is TreeNodeYActCharacterMotion)
            {
                CreateHeader("Character Animation");
                CreateButton("Import", delegate
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "Character Animation|*.omt;*.dat";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        (node as TreeNodeYActCharacterMotion).File.Buffer = File.ReadAllBytes(dialog.FileName);
                    }
                });
                CreateButton("Export", delegate
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "PS2 Animation (*.omt)|*.omt";
                    dialog.FileName = node.Parent.Text + ".omt";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, (node as TreeNodeYActCharacterMotion).File.Buffer);
                    }
                });
            }
            else if (node is TreeNodeYActEffect)
            {
                var effectNode = node as TreeNodeYActEffect;
                CreateHeader("YAct Effect");

                if(!string.IsNullOrEmpty(effectNode.Effect.Name))
                    CreateInput("Name", effectNode.Effect.Name.ToString(), delegate (string val) { effectNode.Effect.Name = val; });

                CreateInput("Start", effectNode.Effect.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { effectNode.Effect.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                CreateInput("End", effectNode.Effect.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { effectNode.Effect.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                CreateInput("Bone ID", effectNode.Effect.BoneID.ToString(), delegate (string val) { effectNode.Effect.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);


                switch ((YActEffectType)effectNode.Effect.Type)
                {
                    case YActEffectType.Particle:
                        var ptcEffect = effectNode.Effect as YActEffectParticle;
                        CreateHeader("Particle");
                        CreateInput("Particle ID", ptcEffect.Particle.ToString(), delegate (string val) { ptcEffect.Particle = int.Parse(val); effectNode.Refresh(); }, NumberBox.NumberMode.Int);
                        break;
                    case YActEffectType.Sound:
                        var soundEffect = effectNode.Effect as YActEffectSound;
                        CreateHeader("Sound");
                        CreateInput("Cuesheet ID", soundEffect.CuesheetID.ToString(), delegate (string val) { soundEffect.CuesheetID = ushort.Parse(val) ; effectNode.Refresh(); }, NumberBox.NumberMode.Ushort);
                        CreateInput("Sound ID", soundEffect.SoundID.ToString(), delegate (string val) { soundEffect.SoundID = ushort.Parse(val); effectNode.Refresh(); }, NumberBox.NumberMode.Ushort);
                        break;
                }


                provider = new Be.Windows.Forms.DynamicByteProvider(effectNode.Effect.UnknownData);
            }

            CreateHeader("");
            unkBytesBox.ByteProvider = provider;
            varPanel.ResumeLayout();
        }

        public void ProcessSelectedNodePS2Prop()
        {
            TreeNode node = nodesTree.SelectedNode;

            Be.Windows.Forms.DynamicByteProvider provider = null;

            if(node is TreeNodePS2PropertyData1)
            {
                var dat1Node = node as TreeNodePS2PropertyData1;
                CreateHeader("Property (Data1)");
                CreateInput("Type", dat1Node.Data1.Type.ToString(), delegate (string val) { }, NumberBox.NumberMode.Int, true);
                CreateInput("Start", dat1Node.Data1.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat1Node.Data1.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                CreateInput("End", dat1Node.Data1.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat1Node.Data1.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                if (dat1Node.Data1.UnknownData != null)
                {
                    provider = new Be.Windows.Forms.DynamicByteProvider(dat1Node.Data1.UnknownData);
                    provider.Changed += delegate
                    {
                        if (provider.Bytes.Count == dat1Node.Data1.UnknownData.Length)
                            dat1Node.Data1.UnknownData = provider.Bytes.ToArray();
                    };
                }
            }
            else if(node is TreeNodePS2PropertyData2)
            {
                var dat2Node = node as TreeNodePS2PropertyData2;
                CreateHeader("Property (Data2)");
                CreateInput("Type?", dat2Node.Data2.Type.ToString(), delegate (string val) {  }, NumberBox.NumberMode.Int, true);
                CreateInput("Start", dat2Node.Data2.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat2Node.Data2.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                CreateInput("End", dat2Node.Data2.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { dat2Node.Data2.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                if (dat2Node.Data2.UnknownData != null)
                {
                    provider = new Be.Windows.Forms.DynamicByteProvider(dat2Node.Data2.UnknownData);
                    provider.Changed += delegate
                    {
                        if (provider.Bytes.Count == dat2Node.Data2.UnknownData.Length)
                            dat2Node.Data2.UnknownData = provider.Bytes.ToArray();
                    };
                }
            }

            CreateHeader("");
            unkBytesBox.ByteProvider = provider;
            varPanel.ResumeLayout();
        }

        private void targetGameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game game = (Game)targetGameCombo.SelectedIndex;

            langOverrideBox.Visible = game >= Game.YK2;
            langOverrideLbl.Visible = game >= Game.YK2;

            if (!langOverrideBox.Visible)
                langOverrideBox.Text = "";
        }

        private void bulkConvertBEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game igame = (Game)targetGameCombo.SelectedIndex;

            if (!CMN.IsDEGame(igame))
                return;

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                List<string> options = new List<string>();


                Game[] deGames = CMN.GetDEGames();

                foreach (Game game in deGames)
                    options.Add(HActLib.Internal.Reflection.GetGamePrefixes(game)[0]);

                string messageString = "Please enter the name of game you want to convert to. \nAvailable:\n";

                foreach (string str in options)
                    messageString += str + "\n";

                string input = Microsoft.VisualBasic.Interaction.InputBox(messageString,
                                "Convert",
                                "",
                                0,
                                0);

                Game prefixGame = CMN.GetGameFromString(input);

                if ((uint)prefixGame >= 9999)
                    return;

                foreach (string bepFile in Directory.GetFiles(dialog.SelectedPath, "*.bep"))
                {
                    BEP bepfile = BEP.Read(bepFile, igame);

                    var inf = RyuseModule.ConvertNodes(bepfile.Nodes.ToArray(), igame, prefixGame);
                    bepfile.Nodes = inf.OutputNodes.ToList();

                    BEP.Write(bepfile, bepFile, prefixGame);
                }

                MessageBox.Show("Complete!");
            }
        }

        private void openYActToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppRegistry.GetFileOpenPath();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            HActDir inf = new HActDir();
            inf.Open(dialog.FileName);

            hactInf = inf;
            folderDir = Path.GetDirectoryName(dialog.FileName);
            FilePath = dialog.FileName;

            yact = BaseYAct.Read(FilePath);

            ProcessYAct(yact);
        }

        private void openPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppRegistry.GetFileOpenPath();
            dialog.CheckFileExists = true;

            DialogResult res = dialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            FilePath = dialog.FileName;
            OMTProperty property = OMTProperty.Read(dialog.FileName);

            ProcessPS2Prop(property);
        }
    }
}

