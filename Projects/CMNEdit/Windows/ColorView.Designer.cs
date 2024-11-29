namespace CMNEdit.Windows
{
    partial class ColorView
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
            panel1 = new System.Windows.Forms.Panel();
            rBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            gBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            bBox = new System.Windows.Forms.TextBox();
            applyButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            aBox = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(191, 252);
            panel1.TabIndex = 0;
            // 
            // rBox
            // 
            rBox.Location = new System.Drawing.Point(231, 64);
            rBox.Name = "rBox";
            rBox.Size = new System.Drawing.Size(46, 23);
            rBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(211, 67);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(14, 15);
            label1.TabIndex = 2;
            label1.Text = "R";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(211, 96);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(15, 15);
            label2.TabIndex = 4;
            label2.Text = "G";
            // 
            // gBox
            // 
            gBox.Location = new System.Drawing.Point(231, 93);
            gBox.Name = "gBox";
            gBox.Size = new System.Drawing.Size(46, 23);
            gBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(211, 125);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(14, 15);
            label3.TabIndex = 6;
            label3.Text = "B";
            // 
            // bBox
            // 
            bBox.Location = new System.Drawing.Point(231, 122);
            bBox.Name = "bBox";
            bBox.Size = new System.Drawing.Size(46, 23);
            bBox.TabIndex = 5;
            // 
            // applyButton
            // 
            applyButton.Location = new System.Drawing.Point(231, 205);
            applyButton.Name = "applyButton";
            applyButton.Size = new System.Drawing.Size(75, 23);
            applyButton.TabIndex = 7;
            applyButton.Text = "Apply";
            applyButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(231, 234);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(211, 154);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(15, 15);
            label4.TabIndex = 10;
            label4.Text = "A";
            // 
            // aBox
            // 
            aBox.Location = new System.Drawing.Point(231, 151);
            aBox.Name = "aBox";
            aBox.Size = new System.Drawing.Size(46, 23);
            aBox.TabIndex = 9;
            // 
            // ColorView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(340, 288);
            Controls.Add(label4);
            Controls.Add(aBox);
            Controls.Add(cancelButton);
            Controls.Add(applyButton);
            Controls.Add(label3);
            Controls.Add(bBox);
            Controls.Add(label2);
            Controls.Add(gBox);
            Controls.Add(label1);
            Controls.Add(rBox);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Name = "ColorView";
            Text = "ColorView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox rBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox bBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox aBox;
    }
}