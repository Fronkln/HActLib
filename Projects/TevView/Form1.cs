using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using HActLib.OOE;

namespace TevView
{
    public partial class Form1 : Form
    {
        public static TEV TevFile;
        public static MEP Mep;
        private static string m_filePath;

        int rowCount = 1;

        public Form1()
        {
            InitializeComponent();

            varPanel.Controls.Clear();
            varPanel.RowCount = 0;
            varPanel.RowStyles.Clear();

            CreateHeader("tEST");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private TreeNode DrawObject(ITEVObject obj)
        {
            if (obj is ObjectBase)
                return DrawSet1(obj as ObjectBase);
            else if (obj is Set2)
                return DrawSet2(obj as Set2);
            else
                return new TreeNodeEffect(obj as EffectBase);
        }

        private TreeNodeSet1 DrawSet1(ObjectBase set)
        {
            TreeNodeSet1 node = new TreeNodeSet1(set);

            foreach (ITEVObject child in set.Children)
                node.Nodes.Add(DrawObject(child));

            return node;
        }

        private TreeNode DrawSet2(Set2 set)
        {
            TreeNodeSet2 node = new TreeNodeSet2(set);

            return node;
        }

        private TreeNode DrawEffect(EffectBase effect)
        {
            TreeNodeEffect node = new TreeNodeEffect(effect);

            return node;
        }

        private void ProcessSet1Hierarchy()
        {
            treeView1.Nodes.Clear();

            TreeNodeSet1 root = DrawSet1(TevFile.Root);
            treeView1.Nodes.Add(root);
        }


        private TreeNode Set2ToNode(Set2 set)
        {
            return new TreeNodeSet2(set);
        }

        private void ProcessSet2Hierarchy()
        {

            treeView1.Nodes.Clear();

            foreach (Set2 set in TevFile.AllSet2)
            {
                treeView1.Nodes.Add(Set2ToNode(set));
            }
        }

        private void ProcessSet3Hierarchy()
        {

            treeView1.Nodes.Clear();

            foreach (EffectBase set in TevFile.AllEffects)
            {
                treeView1.Nodes.Add(new TreeNodeEffect(set));
            }
        }


        private void set1Btn_Click(object sender, EventArgs e)
        {
            ProcessSet1Hierarchy();
        }

        private void set2Btn_Click(object sender, EventArgs e)
        {
            ProcessSet2Hierarchy();
        }

        private void set3Btn_Click(object sender, EventArgs e)
        {
            ProcessSet3Hierarchy();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ProcessSelectedItem(TreeNode node)
        {
            Be.Windows.Forms.DynamicByteProvider provider = null;
            ClearNodeMenu();

            if (node is TreeNodeSet1)
            {
                TreeNodeSet1 set1Node = (node as TreeNodeSet1);

                provider = new Be.Windows.Forms.DynamicByteProvider(set1Node.Set.Unk1);
                Set1Window.Draw(this, set1Node.Set);
            }
            else if (node is TreeNodeSet2)
            {
                TreeNodeSet2 set2Node = (node as TreeNodeSet2);

                provider = new Be.Windows.Forms.DynamicByteProvider(set2Node.Set.Unk2);
                Set2Window.Draw(this, set2Node.Set);

                if(set2Node.Set.Type == Set2NodeCategory.Element)
                {
                    Set2Element elem = set2Node.Set as Set2Element;


                    if(elem.Effect != null)
                        DrawEffectWindow(elem.Effect);

                    /*
                    switch(elem.EffectID)
                    {
                         

                        case EffectID.Particle:
                            Set2WindowParticle.Draw(this, elem as Set2ElementParticle);
                            break;
                    }
                    */

                }
            }
            else if (node is TreeNodeEffect)
            {
                DrawEffectWindow((node as TreeNodeEffect).Effect);
            }


            unkDat.ByteProvider = provider;
        }


        private void DrawEffectWindow(EffectBase effect)
        {
            EffectWindowBase.Draw(this, effect);
            Be.Windows.Forms.DynamicByteProvider provider = new Be.Windows.Forms.DynamicByteProvider(effect.Data == null ? new byte[0] : effect.Data);

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
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProcessSelectedItem(treeView1.SelectedNode);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
                return;

            set1Btn.Visible = true;
            set2Btn.Visible = true;
            set3Btn.Visible = true;

            m_filePath = ofd.FileName;

            TevFile = TEV.Read(ofd.FileName);
            Mep = null;

            ProcessSet1Hierarchy();
        }

        private void saveTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mep == null && TevFile == null)
                return;

            if (Mep != null)
               MEP.Write(Mep, m_filePath);
            else
                TEV.Write(TevFile, m_filePath);
        }

        private void unkDat_Click(object sender, EventArgs e)
        {

        }

        private void GenerateTEV()
        {
            TEV tev = new TEV();
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

        private void openMEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (string.IsNullOrEmpty(ofd.FileName))
                return;

            m_filePath = ofd.FileName;

            Mep = MEP.Read(ofd.FileName);
            TevFile = null;

            ClearNodeMenu();
            treeView1.Nodes.Clear();

            set1Btn.Visible = false;
            set2Btn.Visible = false;
            set3Btn.Visible = false;

            foreach (MepEffectY3 effect in Mep.Effects)
                treeView1.Nodes.Add(new TreeNodeEffect(effect.Effect));
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (treeView1.SelectedNode is TreeNodeSet2)
                    throw new Exception("kill");
            }
        }
    }
}
