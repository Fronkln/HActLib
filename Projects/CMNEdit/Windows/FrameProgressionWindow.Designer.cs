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
            SuspendLayout();
            // 
            // cameraList
            // 
            cameraList.FormattingEnabled = true;
            cameraList.Location = new System.Drawing.Point(12, 12);
            cameraList.Name = "cameraList";
            cameraList.Size = new System.Drawing.Size(211, 79);
            cameraList.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(242, 89);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(138, 23);
            button1.TabIndex = 5;
            button1.Text = "Delete Selected";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(242, 60);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(138, 23);
            button2.TabIndex = 6;
            button2.Text = "Insert After Selected";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(242, 28);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(138, 26);
            button3.TabIndex = 7;
            button3.Text = "Overwrite Selected";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(242, 194);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(138, 23);
            button4.TabIndex = 8;
            button4.Text = "Copy Progression";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(242, 223);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(138, 23);
            button5.TabIndex = 9;
            button5.Text = "Paste Progression";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // frameProgression
            // 
            frameProgression.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            frameProgression.Location = new System.Drawing.Point(14, 97);
            frameProgression.Name = "frameProgression";
            frameProgression.Size = new System.Drawing.Size(209, 311);
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
            button6.Location = new System.Drawing.Point(242, 141);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(138, 23);
            button6.TabIndex = 11;
            button6.Text = "Adjust Selected";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(242, 252);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(138, 23);
            button7.TabIndex = 12;
            button7.Text = "Export Progression";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(242, 281);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(138, 23);
            button8.TabIndex = 13;
            button8.Text = "Clipboard All";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // FrameProgressionWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(392, 431);
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
    }
}
