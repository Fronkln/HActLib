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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            panel = new System.Windows.Forms.Panel();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            startRangeBox = new System.Windows.Forms.TextBox();
            endRangeBox = new System.Windows.Forms.TextBox();
            customValueBox = new System.Windows.Forms.TextBox();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            copyCurveButton = new System.Windows.Forms.Button();
            pasteCurveButton = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
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
            panel.AutoScroll = true;
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Location = new System.Drawing.Point(3, 12);
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size(200, 517);
            panel.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(231, 189);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(137, 23);
            button1.TabIndex = 7;
            button1.Text = "Set Range to 0";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(231, 218);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(137, 23);
            button2.TabIndex = 8;
            button2.Text = "Set Range to 0.5";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(231, 247);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(137, 23);
            button3.TabIndex = 9;
            button3.Text = "Set Range to 1";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(231, 137);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(137, 23);
            button4.TabIndex = 10;
            button4.Text = "Set Range to Value";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // startRangeBox
            // 
            startRangeBox.Location = new System.Drawing.Point(243, 50);
            startRangeBox.Name = "startRangeBox";
            startRangeBox.Size = new System.Drawing.Size(30, 23);
            startRangeBox.TabIndex = 13;
            startRangeBox.Text = "0";
            // 
            // endRangeBox
            // 
            endRangeBox.Location = new System.Drawing.Point(321, 50);
            endRangeBox.Name = "endRangeBox";
            endRangeBox.Size = new System.Drawing.Size(30, 23);
            endRangeBox.TabIndex = 14;
            endRangeBox.Text = "0";
            // 
            // customValueBox
            // 
            customValueBox.Location = new System.Drawing.Point(259, 108);
            customValueBox.Name = "customValueBox";
            customValueBox.Size = new System.Drawing.Size(80, 23);
            customValueBox.TabIndex = 15;
            customValueBox.Text = "0";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(418, 32);
            chart1.Name = "chart1";
            chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            chart1.Size = new System.Drawing.Size(486, 421);
            chart1.TabIndex = 16;
            chart1.Text = "chart1";
            chart1.MouseMove += chart1_MouseMove;
            chart1.MouseUp += chart1_MouseUp;
            // 
            // copyCurveButton
            // 
            copyCurveButton.Location = new System.Drawing.Point(231, 314);
            copyCurveButton.Name = "copyCurveButton";
            copyCurveButton.Size = new System.Drawing.Size(137, 23);
            copyCurveButton.TabIndex = 17;
            copyCurveButton.Text = "Copy Curve";
            copyCurveButton.UseVisualStyleBackColor = true;
            copyCurveButton.Click += copyCurveButton_Click;
            // 
            // pasteCurveButton
            // 
            pasteCurveButton.Location = new System.Drawing.Point(231, 343);
            pasteCurveButton.Name = "pasteCurveButton";
            pasteCurveButton.Size = new System.Drawing.Size(137, 23);
            pasteCurveButton.TabIndex = 18;
            pasteCurveButton.Text = "Paste Curve";
            pasteCurveButton.UseVisualStyleBackColor = true;
            pasteCurveButton.Click += pasteCurveButton_Click;
            // 
            // CurveView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(950, 541);
            Controls.Add(pasteCurveButton);
            Controls.Add(copyCurveButton);
            Controls.Add(chart1);
            Controls.Add(customValueBox);
            Controls.Add(endRangeBox);
            Controls.Add(startRangeBox);
            Controls.Add(label4);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panel);
            Name = "CurveView";
            Text = "CurveView";
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button copyCurveButton;
        private System.Windows.Forms.Button pasteCurveButton;
    }
}