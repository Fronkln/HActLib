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
            FramesList = new System.Windows.Forms.ListBox();
            startBox = new System.Windows.Forms.TextBox();
            endBox = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            applyButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(160, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(142, 23);
            label1.TabIndex = 1;
            label1.Text = "Start (Including)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(159, 82);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(135, 23);
            label2.TabIndex = 2;
            label2.Text = "End (Excluding)";
            // 
            // FramesList
            // 
            FramesList.FormattingEnabled = true;
            FramesList.Location = new System.Drawing.Point(14, 21);
            FramesList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            FramesList.Name = "FramesList";
            FramesList.Size = new System.Drawing.Size(137, 284);
            FramesList.TabIndex = 0;
            FramesList.SelectedIndexChanged += FramesList_SelectedIndexChanged;
            // 
            // startBox
            // 
            startBox.Location = new System.Drawing.Point(160, 48);
            startBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            startBox.Name = "startBox";
            startBox.Size = new System.Drawing.Size(114, 27);
            startBox.TabIndex = 3;
            // 
            // endBox
            // 
            endBox.Location = new System.Drawing.Point(160, 109);
            endBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            endBox.Name = "endBox";
            endBox.Size = new System.Drawing.Size(114, 27);
            endBox.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(179, 254);
            button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(86, 31);
            button1.TabIndex = 5;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(179, 293);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(86, 31);
            button2.TabIndex = 6;
            button2.Text = "Delete";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // applyButton
            // 
            applyButton.Font = new System.Drawing.Font("Segoe UI", 16F);
            applyButton.Location = new System.Drawing.Point(157, 144);
            applyButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            applyButton.Name = "applyButton";
            applyButton.Size = new System.Drawing.Size(137, 67);
            applyButton.TabIndex = 7;
            applyButton.Text = "Apply";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // DisableFrameWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(314, 367);
            Controls.Add(applyButton);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(endBox);
            Controls.Add(startBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(FramesList);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "DisableFrameWindow";
            Text = "DisableFrameWindow";
            ResumeLayout(false);
            PerformLayout();

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