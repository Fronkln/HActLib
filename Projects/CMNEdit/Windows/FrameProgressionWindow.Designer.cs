using System.Windows.Forms;

namespace Frame_Progression_GUI
{
    partial class FrameProgressionWindow
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
            cameraList = new ListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            frameProgression = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            captionsBox = new ListBox();
            SuspendLayout();
            // 
            // cameraList
            // 
            cameraList.FormattingEnabled = true;
            cameraList.Location = new System.Drawing.Point(14, 16);
            cameraList.Margin = new Padding(3, 4, 3, 4);
            cameraList.Name = "cameraList";
            cameraList.Size = new System.Drawing.Size(241, 104);
            cameraList.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(277, 119);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(158, 31);
            button1.TabIndex = 5;
            button1.Text = "Delete Selected";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(277, 80);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(158, 31);
            button2.TabIndex = 6;
            button2.Text = "Insert After Selected";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(277, 37);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(158, 35);
            button3.TabIndex = 7;
            button3.Text = "Overwrite Selected";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(277, 259);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(158, 31);
            button4.TabIndex = 8;
            button4.Text = "Copy Progression";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(277, 297);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(158, 31);
            button5.TabIndex = 9;
            button5.Text = "Paste Progression";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // frameProgression
            // 
            frameProgression.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            frameProgression.Location = new System.Drawing.Point(16, 129);
            frameProgression.Margin = new Padding(3, 4, 3, 4);
            frameProgression.Name = "frameProgression";
            frameProgression.Size = new System.Drawing.Size(238, 413);
            frameProgression.TabIndex = 10;
            frameProgression.UseCompatibleStateImageBehavior = false;
            frameProgression.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Frame";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Speed Change";
            columnHeader2.Width = 100;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(277, 188);
            button6.Margin = new Padding(3, 4, 3, 4);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(158, 31);
            button6.TabIndex = 11;
            button6.Text = "Adjust Selected";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(277, 473);
            button7.Margin = new Padding(3, 4, 3, 4);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(158, 31);
            button7.TabIndex = 12;
            button7.Text = "Export Progression";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(277, 512);
            button8.Margin = new Padding(3, 4, 3, 4);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(158, 31);
            button8.TabIndex = 13;
            button8.Text = "Clipboard All";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // captionsBox
            // 
            captionsBox.FormattingEnabled = true;
            captionsBox.Location = new System.Drawing.Point(16, 550);
            captionsBox.Margin = new Padding(3, 4, 3, 4);
            captionsBox.Name = "captionsBox";
            captionsBox.Size = new System.Drawing.Size(241, 104);
            captionsBox.TabIndex = 14;
            // 
            // FrameProgressionWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(448, 725);
            Controls.Add(captionsBox);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(frameProgression);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(cameraList);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrameProgressionWindow";
            Text = "Frame Progression";
            TopMost = true;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ListBox cameraList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListView frameProgression;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private ListBox captionsBox;
    }
}
