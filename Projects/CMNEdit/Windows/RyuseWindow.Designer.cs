namespace CMNEdit.Windows
{
    partial class RyuseWindow
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
            targetGameBox = new System.Windows.Forms.ComboBox();
            convertParticleCheck = new System.Windows.Forms.CheckBox();
            particleParBox = new System.Windows.Forms.TextBox();
            particleOutputBox = new System.Windows.Forms.TextBox();
            convertButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            outputBrowseDir = new System.Windows.Forms.Button();
            particleParBrowseDir = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // targetGameBox
            // 
            targetGameBox.FormattingEnabled = true;
            targetGameBox.Location = new System.Drawing.Point(29, 51);
            targetGameBox.Name = "targetGameBox";
            targetGameBox.Size = new System.Drawing.Size(121, 23);
            targetGameBox.TabIndex = 0;
            // 
            // convertParticleCheck
            // 
            convertParticleCheck.AutoSize = true;
            convertParticleCheck.Location = new System.Drawing.Point(173, 33);
            convertParticleCheck.Name = "convertParticleCheck";
            convertParticleCheck.Size = new System.Drawing.Size(110, 19);
            convertParticleCheck.TabIndex = 1;
            convertParticleCheck.Text = "Convert Particle";
            convertParticleCheck.UseVisualStyleBackColor = true;
            convertParticleCheck.CheckedChanged += convertParticleCheck_CheckedChanged;
            // 
            // particleParBox
            // 
            particleParBox.Location = new System.Drawing.Point(173, 73);
            particleParBox.Name = "particleParBox";
            particleParBox.ReadOnly = true;
            particleParBox.Size = new System.Drawing.Size(434, 23);
            particleParBox.TabIndex = 2;
            // 
            // particleOutputBox
            // 
            particleOutputBox.Location = new System.Drawing.Point(173, 145);
            particleOutputBox.Name = "particleOutputBox";
            particleOutputBox.ReadOnly = true;
            particleOutputBox.Size = new System.Drawing.Size(434, 23);
            particleOutputBox.TabIndex = 3;
            particleOutputBox.TextChanged += particleOutputBox_TextChanged;
            // 
            // convertButton
            // 
            convertButton.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            convertButton.Location = new System.Drawing.Point(29, 273);
            convertButton.Name = "convertButton";
            convertButton.Size = new System.Drawing.Size(129, 52);
            convertButton.TabIndex = 4;
            convertButton.Text = "Convert";
            convertButton.UseVisualStyleBackColor = true;
            convertButton.Click += convertButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(173, 55);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(93, 15);
            label1.TabIndex = 5;
            label1.Text = "Particle.par path";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(173, 127);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 15);
            label2.TabIndex = 6;
            label2.Text = "Output directory";
            // 
            // outputBrowseDir
            // 
            outputBrowseDir.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            outputBrowseDir.Location = new System.Drawing.Point(173, 174);
            outputBrowseDir.Name = "outputBrowseDir";
            outputBrowseDir.Size = new System.Drawing.Size(56, 22);
            outputBrowseDir.TabIndex = 7;
            outputBrowseDir.Text = "...";
            outputBrowseDir.UseVisualStyleBackColor = true;
            outputBrowseDir.Click += outputBrowseDir_Click;
            // 
            // particleParBrowseDir
            // 
            particleParBrowseDir.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            particleParBrowseDir.Location = new System.Drawing.Point(173, 102);
            particleParBrowseDir.Name = "particleParBrowseDir";
            particleParBrowseDir.Size = new System.Drawing.Size(56, 22);
            particleParBrowseDir.TabIndex = 8;
            particleParBrowseDir.Text = "...";
            particleParBrowseDir.UseVisualStyleBackColor = true;
            particleParBrowseDir.Click += particleParBrowseDir_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(29, 33);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(83, 15);
            label3.TabIndex = 9;
            label3.Text = "Output Game";
            // 
            // RyuseWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(634, 360);
            Controls.Add(label3);
            Controls.Add(particleParBrowseDir);
            Controls.Add(outputBrowseDir);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(convertButton);
            Controls.Add(particleOutputBox);
            Controls.Add(particleParBox);
            Controls.Add(convertParticleCheck);
            Controls.Add(targetGameBox);
            Name = "RyuseWindow";
            Text = "RyuseWindow";
            FormClosed += RyuseWindow_FormClosed;
            Load += RyuseWindow_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox targetGameBox;
        private System.Windows.Forms.CheckBox convertParticleCheck;
        private System.Windows.Forms.TextBox particleParBox;
        private System.Windows.Forms.TextBox particleOutputBox;
        private System.Windows.Forms.Button convertButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button outputBrowseDir;
        private System.Windows.Forms.Button particleParBrowseDir;
        private System.Windows.Forms.Label label3;
    }
}