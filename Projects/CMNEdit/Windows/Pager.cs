using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    public partial class Pager : Form
    {
        public static AuthPage[] Pages;

        public static TreeNode CopiedNode;
        int rowCount = 1;

        public Pager()
        {
            InitializeComponent();

            varTable.Controls.Clear();
            varTable.RowCount = 0;
            varTable.RowStyles.Clear();

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            ProcessHierarchy();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            ProcessHierarchy();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        public void ProcessHierarchy()
        {
            treeView1.Nodes.Clear();

            if (Form1.Instance.AuthPagesDE != null)
            {
                foreach (AuthPage page in Form1.Instance.AuthPagesDE)
                {
                    TreeViewItemPage pageNode = new TreeViewItemPage(page);
                    treeView1.Nodes.Add(pageNode);
                }
            }

        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.KeyCode == Keys.C)
                {
                    if (treeView1.SelectedNode != null)
                        CopiedNode = treeView1.SelectedNode;
                }

                if (e.KeyCode == Keys.V)
                {
                    if (CopiedNode == null)
                        return;

                    switch (CopiedNode.GetType().Name)
                    {
                        case "TreeViewItemPage":
                            List<AuthPage> pagesList = Form1.Instance.AuthPagesDE.ToList();
                            pagesList.Add((CopiedNode as TreeViewItemPage).Page.Copy());
                            pagesList[pagesList.Count - 1].PageIndex = pagesList.Count - 1;
                            ClearVarTable();
                            Form1.Instance.AuthPagesDE = pagesList.ToArray();

                            ProcessHierarchy();

                            break;

                        case "TreeViewItemTransition":
                            if (treeView1.SelectedNode != null && treeView1.SelectedNode is TreeViewItemPage)
                            {
                                Transition newTrans = (CopiedNode as TreeViewItemTransition).Transition.Copy();

                                List<Transition> transitions = (treeView1.SelectedNode as TreeViewItemPage).Page.Transitions.ToList();
                                transitions.Add(newTrans);

                                (treeView1.SelectedNode as TreeViewItemPage).Page.Transitions = transitions.ToList();
                                int idx = treeView1.Nodes.IndexOf(treeView1.SelectedNode);

                                ProcessHierarchy();

                                treeView1.Nodes[idx].Expand();
                            }
                            break;
                        case "TreeViewItemCondition":
                            if (treeView1.SelectedNode != null && treeView1.SelectedNode is TreeViewItemTransition)
                            {
                                Condition newCond = (CopiedNode as TreeViewItemCondition).Condition.Copy();

                                List<Condition> conditions = (treeView1.SelectedNode as TreeViewItemTransition).Transition.Conditions.ToList();
                                conditions.Add(newCond);

                                (treeView1.SelectedNode as TreeViewItemTransition).Transition.Conditions = conditions.ToList();
                                ProcessHierarchy();
                            }
                            break;
                    }

                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (treeView1.SelectedNode is TreeViewItemPage)
                {
                    List<AuthPage> pagesList = Form1.Instance.AuthPagesDE.ToList();
                    pagesList.Remove((treeView1.SelectedNode as TreeViewItemPage).Page);

                    ClearVarTable();

                    Form1.Instance.AuthPagesDE = pagesList.ToArray();

                    ProcessHierarchy();
                }
                else if (treeView1.SelectedNode is TreeViewItemTransition)
                {
                    TreeViewItemTransition transNode = treeView1.SelectedNode as TreeViewItemTransition;

                    List<Transition> transitionList = transNode.Page.Transitions.ToList();
                    transitionList.Remove(transNode.Transition);

                    ClearVarTable();

                    transNode.Page.Transitions = transitionList.ToList();

                    ProcessHierarchy();
                }
                else if (treeView1.SelectedNode is TreeViewItemCondition)
                {
                    TreeViewItemCondition condNode = treeView1.SelectedNode as TreeViewItemCondition;

                    List<Condition> conditionList = condNode.Transition.Conditions.ToList();
                    conditionList.Remove(condNode.Condition);

                    ClearVarTable();

                    condNode.Transition.Conditions = conditionList.ToList();

                    ProcessHierarchy();
                }
            }
        }

        private void movePageUp_Click(object sender, EventArgs e)
        {
            TreeViewItemPage page = (treeView1.SelectedNode as TreeViewItemPage);

            if (Form1.Instance.AuthPagesDE.Length <= 1)
                return;

            if (page == null)
                return;

            int idx =  Array.IndexOf(Form1.Instance.AuthPagesDE, page.Page);
            int newIdx = -1;

            if (idx == 0)
                newIdx = SwapPages(page.Page, Form1.Instance.AuthPagesDE[Form1.Instance.AuthPagesDE.Length - 1]);
            else
                newIdx = SwapPages(page.Page, Form1.Instance.AuthPagesDE[idx - 1]);

            if (newIdx != -1)
            {
                ProcessHierarchy();
                treeView1.SelectedNode = treeView1.Nodes[newIdx];
            }
        }

        private void movePageDown_Click(object sender, EventArgs e)
        {
            TreeViewItemPage page = (treeView1.SelectedNode as TreeViewItemPage);

            if (Form1.Instance.AuthPagesDE.Length <= 1)
                return;

            if (page == null)
                return;

            int idx = Array.IndexOf(Form1.Instance.AuthPagesDE, page.Page);
            int newIdx = -1;

            if (idx == Form1.Instance.AuthPagesDE.Length - 1)
                newIdx = SwapPages(page.Page, Form1.Instance.AuthPagesDE[0]);
            else
                newIdx = SwapPages(page.Page, Form1.Instance.AuthPagesDE[idx + 1]);

            if (newIdx != -1)
            {
                ProcessHierarchy();
                treeView1.SelectedNode = treeView1.Nodes[newIdx];
            }
        }

        private int SwapPages(AuthPage replacer, AuthPage replacing)
        {
            int replacerIdx = Array.IndexOf(Form1.Instance.AuthPagesDE, replacer);
            int replacingIdx = Array.IndexOf(Form1.Instance.AuthPagesDE, replacing);


            Form1.Instance.AuthPagesDE[replacingIdx] = replacer;
            Form1.Instance.AuthPagesDE[replacerIdx] = replacing;

            replacer.PageIndex = replacingIdx;
            replacing.PageIndex = replacerIdx;

            return replacingIdx;
        }

        void ClearVarTable()
        {
            varTable.Controls.Clear();
            varTable.RowStyles.Clear();
            rowCount = 1;
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

        public void CreateHeader(string label, float spacing = 0)
        {
            Label label2 = new Label();
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point);
            //  label2.Location = new Point(42, 5);
            label2.Size = new Size(195, 10);
            label2.TabIndex = 0;
            label2.Text = label;

            if (spacing > 0)
            {
                varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, spacing));
                rowCount++;
                varTable.RowCount = rowCount;

                varTable.Controls.Add(CreateText(""), 0, varTable.RowCount - 2);
                varTable.Controls.Add(CreateText(""), 1, varTable.RowCount - 2);
            }
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            rowCount++;
            varTable.RowCount = rowCount;

            varTable.Controls.Add(label2, 0, varTable.RowCount - 2);
            varTable.Controls.Add(CreateText(""), 1, varTable.RowCount - 2);
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        }

        public void CreateSpace(float space)
        {
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, space));
            rowCount++;
            varTable.RowCount = rowCount;

            varTable.Controls.Add(CreateText(""), 0, varTable.RowCount - 2);
            varTable.Controls.Add(CreateText(""), 1, varTable.RowCount - 2);
        }

        public void CreateSpace(bool big)
        {
            if (big)
                CreateHeader("");
        }

        public TextBox CreateInput(string label, string defaultValue, Action<string> editedCallback, NumberBox.NumberMode mode = NumberBox.NumberMode.Text, bool readOnly = false)
        {
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varTable.RowCount = rowCount;

            NumberBox input = new NumberBox(mode, editedCallback);
            input.Text = defaultValue;
            input.Size = new Size(200, 15);
            input.ReadOnly = readOnly;

            varTable.Controls.Add(CreateText(label), 0, varTable.RowCount - 2);
            varTable.Controls.Add(input, 1, varTable.RowCount - 2);
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));

            return input;
        }

        public void CreateComboBox(string label, int defaultIndex, string[] items, Action<int> editedCallback)
        {
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            rowCount++;
            varTable.RowCount = rowCount;

            ComboBox input = new ComboBox();
            input.Items.AddRange(items);
            input.SelectedIndex = defaultIndex;
            input.Size = new Size(200, 15);

            input.SelectedIndexChanged += delegate { editedCallback?.Invoke(input.SelectedIndex); };

            varTable.Controls.Add(CreateText(label), 0, varTable.RowCount - 2);
            varTable.Controls.Add(input, 1, varTable.RowCount - 2);
            varTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            ClearVarTable();

            switch (treeView1.SelectedNode.GetType().Name)
            {
                

                case "TreeViewItemPage":
                    PageWindow.Draw(this, treeView1.SelectedNode as TreeViewItemPage);
                    break;
                case "TreeViewItemTransition":
                    TransitionWindow.Draw(this, treeView1.SelectedNode as TreeViewItemTransition);
                    break;
                case "TreeViewItemCondition":
                    ConditionWindow.Draw(this, treeView1.SelectedNode as TreeViewItemCondition);
                    break;

            }
        }
    }
}
