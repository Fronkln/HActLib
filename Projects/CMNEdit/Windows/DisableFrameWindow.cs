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
    public partial class DisableFrameWindow : Form
    {
        List<DisableFrameInfo> frameList = new List<DisableFrameInfo>();

        public DisableFrameWindow()
        {
            InitializeComponent();
        }

        public void Init(DisableFrameInfo[] inf)
        {
            frameList = inf.ToList();
            GenerateList();
        }

        private void GenerateList()
        {
            FramesList.Items.Clear();

            foreach (DisableFrameInfo inf in frameList)
                FramesList.Items.Add($"{inf.Start}-{inf.End}");
        }

        private void FramesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FramesList.SelectedIndex < 0)
                return;

            DisableFrameInfo selected = frameList[FramesList.SelectedIndex];
            startBox.Text = selected.Start.ToString(CultureInfo.InvariantCulture);
            endBox.Text = selected.End.ToString(CultureInfo.InvariantCulture);
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

            try
            {
                end = Utils.InvariantParse(endBox.Text);
            }
            catch
            {
                end = 0;
            }

            startBox.Text = start.ToString(CultureInfo.InvariantCulture);
            endBox.Text = end.ToString(CultureInfo.InvariantCulture);

            DisableFrameInfo selected = frameList[FramesList.SelectedIndex];
            selected.Start = start;
            selected.End = end;

            FramesList.Items[FramesList.SelectedIndex] = ($"{startBox.Text}-{endBox.Text}");

            Form1.Instance.DisableFrameInfos = frameList.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frameList.Add(new DisableFrameInfo());
            GenerateList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FramesList.SelectedIndex < 0)
                return;

            int idx = FramesList.SelectedIndex;

            FramesList.Items.RemoveAt(idx);
            frameList.RemoveAt(idx);

            Form1.Instance.DisableFrameInfos = frameList.ToArray();

            GenerateList();
        }
    }
}
