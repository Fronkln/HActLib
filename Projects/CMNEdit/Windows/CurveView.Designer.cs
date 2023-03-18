namespace CMNEdit.Windows
{
    partial class CurveView
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.panel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.startRangeBox = new System.Windows.Forms.TextBox();
            this.endRangeBox = new System.Windows.Forms.TextBox();
            this.customValueBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(227, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 15);
            label2.TabIndex = 4;
            label2.Text = "Start Range";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(305, 32);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(63, 15);
            label3.TabIndex = 6;
            label3.Text = "End Range";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(259, 90);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(80, 15);
            label4.TabIndex = 12;
            label4.Text = "Custom Value";
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(3, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(200, 517);
            this.panel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Set Range to 0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(231, 218);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Set Range to 0.5";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(231, 247);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Set Range to 1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(231, 137);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(137, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Set Range to Value";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // startRangeBox
            // 
            this.startRangeBox.Location = new System.Drawing.Point(243, 50);
            this.startRangeBox.Name = "startRangeBox";
            this.startRangeBox.Size = new System.Drawing.Size(30, 23);
            this.startRangeBox.TabIndex = 13;
            this.startRangeBox.Text = "0";
            // 
            // endRangeBox
            // 
            this.endRangeBox.Location = new System.Drawing.Point(321, 50);
            this.endRangeBox.Name = "endRangeBox";
            this.endRangeBox.Size = new System.Drawing.Size(30, 23);
            this.endRangeBox.TabIndex = 14;
            this.endRangeBox.Text = "0";
            // 
            // customValueBox
            // 
            this.customValueBox.Location = new System.Drawing.Point(259, 108);
            this.customValueBox.Name = "customValueBox";
            this.customValueBox.Size = new System.Drawing.Size(80, 23);
            this.customValueBox.TabIndex = 15;
            this.customValueBox.Text = "0";
            // 
            // CurveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 541);
            this.Controls.Add(this.customValueBox);
            this.Controls.Add(this.endRangeBox);
            this.Controls.Add(this.startRangeBox);
            this.Controls.Add(label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.panel);
            this.Name = "CurveView";
            this.Text = "CurveView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox startRangeBox;
        private System.Windows.Forms.TextBox endRangeBox;
        private System.Windows.Forms.TextBox customValueBox;
    }
}