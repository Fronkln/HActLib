using HActLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace CMNEdit.Windows
{
    public partial class ResourceCutWindow : Form
    {
        List<float> resourceCuts = new List<float>();
        public ResourceCutWindow()
        {
            InitializeComponent();
        }

        public void Init(float[] inf)
        {
            resourceCuts = inf.ToList();
            GenerateList();
        }

        private void GenerateList()
        {
            FramesList.Items.Clear();

            foreach (float inf in resourceCuts)
                FramesList.Items.Add($"{inf.ToString(CultureInfo.InvariantCulture)}");
        }

        private void FramesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FramesList.SelectedIndex < 0)
                return;

            float selected = resourceCuts[FramesList.SelectedIndex];
            startBox.Text = selected.ToString(CultureInfo.InvariantCulture);
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (FramesList.SelectedIndex < 0)
                return;

            float start = 0;
            float end = 0;

            try
            {
                start = Utils.InvariantParse(startBox.Text);
            }
            catch
            {
                start = 0;
            }

            startBox.Text = start.ToString(CultureInfo.InvariantCulture);

            float selected = resourceCuts[FramesList.SelectedIndex];
            resourceCuts[FramesList.SelectedIndex] = start;

            FramesList.Items[FramesList.SelectedIndex] = ($"{startBox.Text}");

            Form1.Instance.ResourceCutInfos = resourceCuts.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resourceCuts.Add(0);
            GenerateList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FramesList.SelectedIndex < 0)
                return;

            int idx = FramesList.SelectedIndex;

            FramesList.Items.RemoveAt(idx);
            resourceCuts.RemoveAt(idx);

            Form1.Instance.ResourceCutInfos = resourceCuts.ToArray();

            GenerateList();
        }
    }
}
