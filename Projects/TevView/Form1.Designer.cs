namespace TevView
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.set1Btn = new System.Windows.Forms.Button();
            this.set2Btn = new System.Windows.Forms.Button();
            this.set3Btn = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCSVTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.unkDat = new Be.Windows.Forms.HexBox();
            this.Node = new System.Windows.Forms.TabPage();
            this.varPanel = new System.Windows.Forms.TableLayoutPanel();
            this.nodeTabs = new System.Windows.Forms.TabControl();
            this.saveCSVTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Node.SuspendLayout();
            this.nodeTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.icons;
            this.treeView1.Location = new System.Drawing.Point(12, 61);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(279, 402);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // icons
            // 
            this.icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons.ImageStream")));
            this.icons.TransparentColor = System.Drawing.Color.Transparent;
            this.icons.Images.SetKeyName(0, "node.png");
            this.icons.Images.SetKeyName(1, "animation.png");
            this.icons.Images.SetKeyName(2, "bone.png");
            this.icons.Images.SetKeyName(3, "camera.png");
            this.icons.Images.SetKeyName(4, "character.png");
            this.icons.Images.SetKeyName(5, "sound.png");
            this.icons.Images.SetKeyName(6, "asset.png");
            this.icons.Images.SetKeyName(7, "particle.png");
            // 
            // set1Btn
            // 
            this.set1Btn.Location = new System.Drawing.Point(12, 32);
            this.set1Btn.Name = "set1Btn";
            this.set1Btn.Size = new System.Drawing.Size(75, 23);
            this.set1Btn.TabIndex = 1;
            this.set1Btn.Text = "Objects";
            this.set1Btn.UseVisualStyleBackColor = true;
            this.set1Btn.Click += new System.EventHandler(this.set1Btn_Click);
            // 
            // set2Btn
            // 
            this.set2Btn.Location = new System.Drawing.Point(93, 32);
            this.set2Btn.Name = "set2Btn";
            this.set2Btn.Size = new System.Drawing.Size(75, 23);
            this.set2Btn.TabIndex = 2;
            this.set2Btn.Text = "Set 2";
            this.set2Btn.UseVisualStyleBackColor = true;
            this.set2Btn.Click += new System.EventHandler(this.set2Btn_Click);
            // 
            // set3Btn
            // 
            this.set3Btn.Location = new System.Drawing.Point(174, 32);
            this.set3Btn.Name = "set3Btn";
            this.set3Btn.Size = new System.Drawing.Size(75, 23);
            this.set3Btn.TabIndex = 3;
            this.set3Btn.Text = "Effects";
            this.set3Btn.UseVisualStyleBackColor = true;
            this.set3Btn.Click += new System.EventHandler(this.set3Btn_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openMEPToolStripMenuItem,
            this.saveTestToolStripMenuItem,
            this.openCSVTestToolStripMenuItem,
            this.saveCSVTestToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openToolStripMenuItem.Text = "Open TEV";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openMEPToolStripMenuItem
            // 
            this.openMEPToolStripMenuItem.Name = "openMEPToolStripMenuItem";
            this.openMEPToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openMEPToolStripMenuItem.Text = "Open MEP";
            this.openMEPToolStripMenuItem.Click += new System.EventHandler(this.openMEPToolStripMenuItem_Click);
            // 
            // saveTestToolStripMenuItem
            // 
            this.saveTestToolStripMenuItem.Name = "saveTestToolStripMenuItem";
            this.saveTestToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveTestToolStripMenuItem.Text = "Save (EXPERIMENTAL)";
            this.saveTestToolStripMenuItem.Click += new System.EventHandler(this.saveTestToolStripMenuItem_Click);
            // 
            // openCSVTestToolStripMenuItem
            // 
            this.openCSVTestToolStripMenuItem.Name = "openCSVTestToolStripMenuItem";
            this.openCSVTestToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openCSVTestToolStripMenuItem.Text = "Open CSV Test";
            this.openCSVTestToolStripMenuItem.Click += new System.EventHandler(this.openCSVTestToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.unkDat);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(435, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Unknown Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // unkDat
            // 
            this.unkDat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.unkDat.Location = new System.Drawing.Point(0, 3);
            this.unkDat.Name = "unkDat";
            this.unkDat.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.unkDat.Size = new System.Drawing.Size(435, 407);
            this.unkDat.TabIndex = 5;
            // 
            // Node
            // 
            this.Node.Controls.Add(this.varPanel);
            this.Node.Location = new System.Drawing.Point(4, 24);
            this.Node.Name = "Node";
            this.Node.Padding = new System.Windows.Forms.Padding(3);
            this.Node.Size = new System.Drawing.Size(435, 403);
            this.Node.TabIndex = 0;
            this.Node.Text = "Data";
            this.Node.UseVisualStyleBackColor = true;
            // 
            // varPanel
            // 
            this.varPanel.AutoScroll = true;
            this.varPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.varPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.varPanel.ColumnCount = 2;
            this.varPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.varPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 317F));
            this.varPanel.Location = new System.Drawing.Point(3, 3);
            this.varPanel.Name = "varPanel";
            this.varPanel.RowCount = 3;
            this.varPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.varPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.varPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.varPanel.Size = new System.Drawing.Size(432, 400);
            this.varPanel.TabIndex = 0;
            // 
            // nodeTabs
            // 
            this.nodeTabs.Controls.Add(this.Node);
            this.nodeTabs.Controls.Add(this.tabPage2);
            this.nodeTabs.Location = new System.Drawing.Point(326, 32);
            this.nodeTabs.Name = "nodeTabs";
            this.nodeTabs.SelectedIndex = 0;
            this.nodeTabs.Size = new System.Drawing.Size(443, 431);
            this.nodeTabs.TabIndex = 7;
            // 
            // saveCSVTestToolStripMenuItem
            // 
            this.saveCSVTestToolStripMenuItem.Name = "saveCSVTestToolStripMenuItem";
            this.saveCSVTestToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveCSVTestToolStripMenuItem.Text = "Save CSV Test";
            this.saveCSVTestToolStripMenuItem.Click += new System.EventHandler(this.saveCSVTestToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 501);
            this.Controls.Add(this.nodeTabs);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.set3Btn);
            this.Controls.Add(this.set2Btn);
            this.Controls.Add(this.set1Btn);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "TevView";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.Node.ResumeLayout(false);
            this.nodeTabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button set1Btn;
        private System.Windows.Forms.Button set2Btn;
        private System.Windows.Forms.Button set3Btn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTestToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage2;
        private Be.Windows.Forms.HexBox unkDat;
        private System.Windows.Forms.TabPage Node;
        private System.Windows.Forms.TableLayoutPanel varPanel;
        private System.Windows.Forms.TabControl nodeTabs;
        private System.Windows.Forms.ImageList icons;
        private System.Windows.Forms.ToolStripMenuItem openMEPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCSVTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCSVTestToolStripMenuItem;
    }
}
