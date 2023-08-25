namespace CMNEdit.Windows
{
    partial class DisableFrameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.FramesList = new System.Windows.Forms.ListBox();
            this.startBox = new System.Windows.Forms.TextBox();
            this.endBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(138, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(80, 37);
            label1.TabIndex = 1;
            label1.Text = "Start";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(138, 82);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 37);
            label2.TabIndex = 2;
            label2.Text = "End";
            // 
            // FramesList
            // 
            this.FramesList.FormattingEnabled = true;
            this.FramesList.ItemHeight = 15;
            this.FramesList.Location = new System.Drawing.Point(12, 16);
            this.FramesList.Name = "FramesList";
            this.FramesList.Size = new System.Drawing.Size(120, 364);
            this.FramesList.TabIndex = 0;
            this.FramesList.SelectedIndexChanged += new System.EventHandler(this.FramesList_SelectedIndexChanged);
            // 
            // startBox
            // 
            this.startBox.Location = new System.Drawing.Point(138, 56);
            this.startBox.Name = "startBox";
            this.startBox.Size = new System.Drawing.Size(100, 23);
            this.startBox.TabIndex = 3;
            // 
            // endBox
            // 
            this.endBox.Location = new System.Drawing.Point(138, 122);
            this.endBox.Name = "endBox";
            this.endBox.Size = new System.Drawing.Size(100, 23);
            this.endBox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 420);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // applyButton
            // 
            this.applyButton.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.applyButton.Location = new System.Drawing.Point(138, 164);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(120, 50);
            this.applyButton.TabIndex = 7;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // DisableFrameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 450);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.endBox);
            this.Controls.Add(this.startBox);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.FramesList);
            this.Name = "DisableFrameWindow";
            this.Text = "DisableFrameWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox FramesList;
        private System.Windows.Forms.TextBox startBox;
        private System.Windows.Forms.TextBox endBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button applyButton;
    }
}