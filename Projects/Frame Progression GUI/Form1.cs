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

namespace Frame_Progression_GUI
{
    public partial class Form1 : Form
    {
        BaseCMN cmn;
        NodeCamera[] m_cameras;

        private List<string> m_copiedProgression = new List<string>();

        public Form1()
        {
            InitializeComponent();
            gameCombobox.Items.AddRange(Enum.GetNames(typeof(Game)));
            gameCombobox.SelectedIndex = 0;

            cameraList.SelectedIndexChanged += delegate { OnSelectedCameraChanged(cameraList.SelectedIndex); };
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game selectedGame = (Game)gameCombobox.SelectedIndex;
            GameVersion gameVersion = CMN.GetVersionForGame(selectedGame);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "*.bin|cmn.bin";
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cameraList.Items.Clear();
                    frameProgression.Items.Clear();


                    string filePath = dialog.FileName;

                    if (CMN.IsDEGame(selectedGame))
                        cmn = CMN.Read(filePath, selectedGame);
                    else
                        cmn = OECMN.Read(filePath);

                    m_cameras = cmn.GetNodes().Where(x => x.Category == AuthNodeCategory.Camera).Cast<NodeCamera>().ToArray();
                    cameraList.Items.AddRange(m_cameras.Select(x => x.Name).ToArray());

                    if (m_cameras.Length <= 0)
                        MessageBox.Show("HAct has no cameras.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        OnSelectedCameraChanged(0);
                }
            }
            catch
            {
                MessageBox.Show("Error opening cmn. Ensure you chose the correct game", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void OnSelectedCameraChanged(int idx)
        {
            frameProgression.Items.Clear();


            NodeCamera camera = m_cameras[idx];
            frameProgression.Items.AddRange(camera.FrameProgression.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
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
    }
}
