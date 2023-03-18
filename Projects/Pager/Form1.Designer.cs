namespace Pager
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.openButton = new System.Windows.Forms.Button();
            this.movePageUp = new System.Windows.Forms.Button();
            this.movePageDown = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.varTable = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.varTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(27, 28);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(206, 337);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(51, -1);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // movePageUp
            // 
            this.movePageUp.Location = new System.Drawing.Point(40, 371);
            this.movePageUp.Name = "movePageUp";
            this.movePageUp.Size = new System.Drawing.Size(86, 23);
            this.movePageUp.TabIndex = 3;
            this.movePageUp.Text = "Move Up";
            this.movePageUp.UseVisualStyleBackColor = true;
            this.movePageUp.Click += new System.EventHandler(this.movePageUp_Click);
            // 
            // movePageDown
            // 
            this.movePageDown.Location = new System.Drawing.Point(132, 371);
            this.movePageDown.Name = "movePageDown";
            this.movePageDown.Size = new System.Drawing.Size(80, 23);
            this.movePageDown.TabIndex = 4;
            this.movePageDown.Text = "Move Down";
            this.movePageDown.UseVisualStyleBackColor = true;
            this.movePageDown.Click += new System.EventHandler(this.movePageDown_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(132, -1);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // varTable
            // 
            this.varTable.AutoScroll = true;
            this.varTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.varTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.varTable.ColumnCount = 2;
            this.varTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.varTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 317F));
            this.varTable.Controls.Add(this.label2, 0, 0);
            this.varTable.Controls.Add(this.label3, 0, 1);
            this.varTable.Controls.Add(this.textBox1, 1, 1);
            this.varTable.Controls.Add(this.panel1, 1, 2);
            this.varTable.Location = new System.Drawing.Point(239, 28);
            this.varTable.Name = "varTable";
            this.varTable.RowCount = 3;
            this.varTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.varTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.varTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.varTable.Size = new System.Drawing.Size(557, 337);
            this.varTable.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(23, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "Basic Information";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "SomeText";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(245, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 23);
            this.textBox1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(245, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 19);
            this.panel1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 408);
            this.Controls.Add(this.varTable);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.movePageDown);
            this.Controls.Add(this.movePageUp);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "Pager";
            this.varTable.ResumeLayout(false);
            this.varTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView treeView1;
        private Button openButton;
        private Button movePageUp;
        private Button movePageDown;
        private Button saveButton;
        private TableLayoutPanel varTable;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private Panel panel1;
    }
}