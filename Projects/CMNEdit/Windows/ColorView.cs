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

namespace CMNEdit.Windows
{
    public partial class ColorView : Form
    {
        private Action<Color> m_finish;

        public ColorView()
        {
            InitializeComponent();
            Form1.Instance.Enabled = false;

            applyButton.Click += delegate 
            { 
                m_finish?.Invoke(panel1.BackColor); 
                Form1.Instance.Enabled = true; 
                Close(); 
            };

            rBox.TextChanged += delegate { try { panel1.BackColor = Color.FromArgb(int.Parse(rBox.Text), panel1.BackColor.G, panel1.BackColor.B); } catch { } };
            gBox.TextChanged += delegate { try { panel1.BackColor = Color.FromArgb(panel1.BackColor.R, int.Parse(gBox.Text), panel1.BackColor.B); } catch { } };
            bBox.TextChanged += delegate { try { panel1.BackColor = Color.FromArgb(panel1.BackColor.R, panel1.BackColor.G, int.Parse(bBox.Text)); } catch { } };

            FormClosed += delegate { Form1.Instance.Enabled = true; };
        }

        public void Init(Color defaultCol, Action<Color> finished)
        {
            panel1.BackColor = defaultCol;
            rBox.Text = defaultCol.R.ToString();
            gBox.Text = defaultCol.G.ToString();
            bBox.Text = defaultCol.B.ToString();

            m_finish = finished;
        }
    }
}
