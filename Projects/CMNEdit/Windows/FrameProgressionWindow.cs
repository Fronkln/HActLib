using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using System.Globalization;
using System.Runtime.InteropServices;
using CMNEdit;
using System.IO;

namespace Frame_Progression_GUI
{
    public partial class FrameProgressionWindow : Form
    {
        private string m_filePath;

        private NodeCamera[] m_cameras;

        private static List<string> m_copiedProgression = new List<string>();

        public NodeCamera SelectedCamera { get { return m_cameras[cameraList.SelectedIndex]; } }

        public FrameProgressionWindow()
        {
            Form1.Instance.Enabled = false;

            InitializeComponent();

            cameraList.Items.Clear();
            frameProgression.Items.Clear();

            cameraList.SelectedIndexChanged += delegate { OnSelectedCameraChanged(cameraList.SelectedIndex); };

            m_cameras = Form1.Instance.GetAllNodes().Where(x => x.Category == AuthNodeCategory.Camera).Cast<NodeCamera>().ToArray();
            cameraList.Items.AddRange(m_cameras.Select(x => x.Name).ToArray());

            if (m_cameras.Length <= 0)
                MessageBox.Show("HAct has no cameras.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                OnSelectedCameraChanged(0);


            FormClosing += delegate { ApplyFrameProgression(); Form1.Instance.Enabled = true; };
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyFrameProgression();
        }


        private void OnSelectedCameraChanged(int idx)
        {
            frameProgression.Items.Clear();

            NodeCamera camera = m_cameras[idx];

            for (int i = 0; i < camera.FrameProgression.Length; i++)
            {
                float f = camera.FrameProgression[i];

                ListViewItem item = new ListViewItem(Math.Round(f, 3).ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(Math.Round(camera.FrameProgressionSpeed[i], 3).ToString(CultureInfo.InvariantCulture));

                frameProgression.Items.Add(item);
            }
            //   listView1.Items.AddRange((camera.FrameProgression.Select(x => x.ToString(CultureInfo.InvariantCulture)).Select(x => new ListViewItem(x)).ToArray()));
            // listView1.Items.subAddRange((camera.FrameProgression.Select(x => x.ToString(CultureInfo.InvariantCulture)).Select(x => new ListViewItem(x)).ToArray()));
            cameraList.SelectedIndex = idx;
        }

        private void frameProgression_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.KeyCode == Keys.C || e.KeyCode == Keys.V)
                {
                    if (e.KeyCode == Keys.C)
                    {

                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frameProgression.SelectedItems.Count < 0)
                return;

            List<ListViewItem> selectedItems = frameProgression.SelectedItems.Cast<ListViewItem>().ToList();

            foreach (var item in selectedItems)
                frameProgression.Items.Remove(item);

            ApplyFrameProgression();
        }


        private void ApplyFrameProgression()
        {
            SelectedCamera.FrameProgression = frameProgression.Items.Cast<ListViewItem>().Select(x => float.Parse(x.Text, CultureInfo.InvariantCulture)).ToArray();


            float[] frameProgressionSpeed = new float[SelectedCamera.FrameProgression.Length];


            if (frameProgressionSpeed.Length <= 0)
                return;

            for (int i = 0; i < frameProgression.Items.Count - 1; i++)
                frameProgressionSpeed[i] = SelectedCamera.FrameProgression[i + 1] - SelectedCamera.FrameProgression[i];

            if (frameProgressionSpeed.Length > 1)
                frameProgressionSpeed[frameProgressionSpeed.Length - 1] = 1;

            SelectedCamera.FrameProgressionSpeed = frameProgressionSpeed;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (frameProgression.SelectedIndices.Count < 0)
                return;

            string frameAmountStr = Microsoft.VisualBasic.Interaction.InputBox("Amount of new frames to add",
           "Frame Count",
           "10",
           0,
           0);

            if (string.IsNullOrEmpty(frameAmountStr))
                return;

            string startValueStr = Microsoft.VisualBasic.Interaction.InputBox("Value of the first frame",
           "First Frame Value",
           "",
           0,
           0);

            if (string.IsNullOrEmpty(startValueStr))
                return;


            string incrementStr = Microsoft.VisualBasic.Interaction.InputBox("The amount to increment every frame (1, 1.5, 0.5 etc)",
           "Increment Amount",
           "1",
           0,
           0);



            int frameCount = int.Parse(frameAmountStr);

            if (frameCount <= 0)
                return;

            float startValue = float.Parse(startValueStr);
            float increment = float.Parse(incrementStr);

            int curIdx = frameProgression.SelectedIndices[0] + 1;
            float curVal = startValue;

            for (int i = 0; i < frameCount; i++)
            {
                frameProgression.Items.Insert(curIdx, curVal.ToString(CultureInfo.InvariantCulture));
                curIdx++;
                curVal += increment;
            }

            //frameProgression.Items.Insert(frameProgression.SelectedIndex, newProg);
            ApplyFrameProgression();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (frameProgression.SelectedIndices.Count < 0)
                return;

            string startValueStr = Microsoft.VisualBasic.Interaction.InputBox("Value of the first frame",
           "First Frame Value",
           "",
           0,
           0);

            if (string.IsNullOrEmpty(startValueStr))
                return;


            string incrementStr = Microsoft.VisualBasic.Interaction.InputBox("The amount to increment every frame (1, 1.5, 0.5 etc)",
           "Increment Amount",
           "1",
           0,
           0);


            int min = frameProgression.SelectedIndices.Cast<int>().Min();
            int max = frameProgression.SelectedIndices.Cast<int>().Max();

            int frameCount = (max - min) + 1;

            if (frameCount <= 0)
                return;

            float startValue = float.Parse(startValueStr);
            float increment = float.Parse(incrementStr);

            int curIdx = min;
            float curVal = startValue;

            for (int i = 0; i < frameCount; i++)
            {
                frameProgression.Items[curIdx].Text = curVal.ToString(CultureInfo.InvariantCulture);
                curIdx++;
                curVal += increment;
            }

            //frameProgression.Items.Insert(frameProgression.SelectedIndex, newProg);
            ApplyFrameProgression();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            m_copiedProgression.Clear();
            m_copiedProgression.AddRange(frameProgression.SelectedItems.Cast<ListViewItem>().Select(x => x.Text));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (string item in m_copiedProgression)
            {
                frameProgression.Items.Add(item);
            }

            ApplyFrameProgression();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string adjustVal = Microsoft.VisualBasic.Interaction.InputBox("Amount to adjust frames by",
            "Adjust Selected Value",
            "0",
            0,
            0);

            float val = CMNEdit.Utils.InvariantParse(adjustVal);

            int min = frameProgression.SelectedIndices.Cast<int>().Min();
            int max = frameProgression.SelectedIndices.Cast<int>().Max();

            int frameCount = (max - min) + 1;

            if (frameCount <= 0)
                return;

            int curIdx = min;

            for (int i = 0; i < frameCount; i++)
            {
                float curVal = CMNEdit.Utils.InvariantParse(frameProgression.Items[i].Text);
                float newVal = curVal + val;
                frameProgression.Items[i].Text = newVal.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.Title = "Export Frame Progression";
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Frame | Speed Change");
                    sb.AppendLine("-------------------");
                    
                    foreach (ListViewItem item in frameProgression.Items)
                    {
                        sb.AppendLine($"{item.Text} | {item.SubItems[1].Text}");
                    }
                    
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                    MessageBox.Show("Frame progression exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting frame progression: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Frame | Speed Change");
                sb.AppendLine("-------------------");
                
                foreach (ListViewItem item in frameProgression.Items)
                {
                    sb.AppendLine($"{item.Text} | {item.SubItems[1].Text}");
                }
                
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("Frame progression copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
