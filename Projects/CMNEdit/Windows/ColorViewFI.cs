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
    public partial class ColorViewFI : Form
    {
        private Action<RGB> m_finish;

        RGB color;

        public ColorViewFI()
        {
            InitializeComponent();
            Form1.Instance.Enabled = false;

            applyButton.Click += delegate 
            { 
                m_finish?.Invoke(panel1.BackColor); 
                Form1.Instance.Enabled = true; 
                Close(); 
            };

            rBox.TextChanged += delegate {
                try 
                {
                    color.R = Utils.InvariantParse(rBox.Text);
                    panel1.BackColor = (Color)color.Clamp();
                } catch { } };
            gBox.TextChanged += delegate { 
                try 
                {
                    color.G = Utils.InvariantParse(gBox.Text);
                    panel1.BackColor = (Color)color.Clamp();
                } catch { } };
            bBox.TextChanged += delegate { 
                try 
                {
                    color.B = Utils.InvariantParse(bBox.Text);
                    panel1.BackColor = (Color)color.Clamp();
                } catch { } };

            FormClosed += delegate { Form1.Instance.Enabled = true; };
        }

        public void Init(RGB defaultCol, Action<RGB> finished)
        {
            color = defaultCol;
            panel1.BackColor = color.Clamp();
            rBox.Text = defaultCol.R.ToString();
            gBox.Text = defaultCol.G.ToString();
            bBox.Text = defaultCol.B.ToString();

            m_finish = finished;
        }
    }
}
