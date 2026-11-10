using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class PropertyEditorBuilder
    {
        public static void CreateHeader(string label, float spacing = 0)
        {
            TableLayoutPanel varPanel = null;
            bool isCsvTree = Form1.Instance.hactTabs.SelectedTab.Text == "CSV";

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static Control CreateText(string label, bool left = false, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static void CreateSpace(float space, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, space));
            varPanel.RowCount++;

            varPanel.Controls.Add(CreateText("", isCsvTree), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(CreateText("", isCsvTree), 1, varPanel.RowCount - 1);
        }

        public static void CreateSpace(bool big)
        {
            TableLayoutPanel varPanel = null;

            bool isCsvTree = Form1.Instance.hactTabs.SelectedTab.Text == "CSV";

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

            if (big)
                CreateHeader("", 0);
        }

        public static TextBox CreateInput(string label, string defaultValue, Action<string> editedCallback, NumberBox.NumberMode mode = NumberBox.NumberMode.Text, bool readOnly = false, bool hex = false)
        {
            TableLayoutPanel varPanel = null;

            bool isCsvTree = Form1.Instance.hactTabs.SelectedTab.Text == "CSV";

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

            NumberBox input = new NumberBox(mode, editedCallback, hex);
            input.Text = defaultValue;
            input.Size = new Size(250, 15);
            input.ReadOnly = readOnly;
            input.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            Control text = CreateText(label, false);

            varPanel.Controls.Add(text, 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public static TextBox CreateMultilineInput(string label, string defaultValue, Action<string> editedCallback, int size = 25, NumberBox.NumberMode mode = NumberBox.NumberMode.Text, bool readOnly = false)
        {
            TableLayoutPanel varPanel = null;

            bool isCsvTree = Form1.Instance.hactTabs.SelectedTab.Text == "CSV";

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, size + 10));
            varPanel.RowCount++;

            NumberBox input = new NumberBox(mode, editedCallback);
            input.Text = defaultValue;
            input.Size = new Size(250, size);
            input.ReadOnly = readOnly;
            input.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            input.Multiline = true;

            Control text = CreateText(label, false);

            varPanel.Controls.Add(text, 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public static Button CreateButton(string text, Action clicked, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            isCsvTree = Form1.Instance.hactTabs.SelectedTab.Text == "CSV";

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static Panel CreatePanel(string label, Color color, Action<Color> finished, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static Panel CreatePanelFI(string label, Color color, Action<HActLib.RGB> finished, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static Panel CreatePanelFI2(string label, Color panelColor, HActLib.RGB color, Action<HActLib.RGB> finished, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

            varPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            varPanel.RowCount++;

            Panel input = new Panel();
            input.BorderStyle = BorderStyle.Fixed3D;
            input.Size = new Size(200, 50);
            input.Click += delegate
            {
                CMNEdit.Windows.ColorViewFI myNewForm = new CMNEdit.Windows.ColorViewFI();
                myNewForm.Visible = true;
                myNewForm.Init(color, finished);
            };
            input.BackColor = panelColor;

            varPanel.Controls.Add(CreateText(label), 0, varPanel.RowCount - 1);
            varPanel.Controls.Add(input, 1, varPanel.RowCount - 1);

            return input;
        }

        public static void CreateComboBox(string label, int defaultIndex, string[] items, Action<int> editedCallback, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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

        public static void CreateHexBox(byte[] buf, bool isCsvTree = false)
        {
            TableLayoutPanel varPanel = null;

            if (!isCsvTree)
                varPanel = Form1.Instance.varPanel;
            else
                varPanel = Form1.Instance.csvVarPanel;

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
    }
}
