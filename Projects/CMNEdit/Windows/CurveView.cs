using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit.Windows
{
    public partial class CurveView : Form
    {
        private const int m_labelX = 15;
        private const int m_textboxX = 45;

        private int m_LabelCurY;
        private int m_TextboxCurY;

        private TextBox[] m_textBoxes = new TextBox[0];
        private Label[] m_labels = new Label[0];

        private byte[] m_curve;
        private float[] m_float_curve;

        private bool is_byte_curve;

        private Action<byte[]> m_finishedCallback = null;
        private Action<float[]> m_finishedCallback_float = null;

        public CurveView()
        {
            InitializeComponent();

            Form1.Instance.Enabled = false;
        }

        private void Init()
        {
            SuspendLayout();

            foreach (TextBox box in m_textBoxes)
                panel.Controls.Remove(box);

            foreach (Label lbl in m_labels)
                panel.Controls.Remove(lbl);

            m_TextboxCurY = 0;
            m_LabelCurY = 0;

            ResumeLayout(true);

            FormClosed += delegate { OnClose(); };
        }

        public void Init(byte[] curve, Action<byte[]> finished)
        {
            Init();

            m_curve = curve;
            m_finishedCallback = finished;

            m_textBoxes = new TextBox[curve.Length];
            m_labels = new Label[curve.Length];

            SuspendLayout();

            for(int i = 0; i < curve.Length; i++)
            {
                byte bit = curve[i];

                Label label = new Label();
                label.Text = i.ToString();
                label.Size = new System.Drawing.Size(30, 15);
                label.Location = new Point(m_labelX, m_LabelCurY);

                int idx = i;
                TextBox box = new NumberBox(NumberBox.NumberMode.Float, delegate (string val) { SetValue(idx, val); });
                box.Text = (bit / 255f).ToString("0.00");
                box.Location = new Point(m_textboxX, m_TextboxCurY);

                m_LabelCurY += 30;
                m_TextboxCurY += 30;

                panel.Controls.Add(label);
                panel.Controls.Add(box);

                m_textBoxes[i] = box;
                m_labels[i] = label;
            }

            ResumeLayout(true);

            FormClosed += delegate { OnClose(); };
        }

        public void Init(float[] curve, Action<float[]> finished)
        {
            Init();

            m_float_curve = curve;
            m_finishedCallback_float = finished;

            m_textBoxes = new TextBox[curve.Length];
            m_labels = new Label[curve.Length];

            for (int i = 0; i < curve.Length; i++)
            {
                Label label = new Label();
                label.Text = i.ToString();
                label.Size = new System.Drawing.Size(30, 15);
                label.Location = new Point(m_labelX, m_LabelCurY);

                int idx = i;
                TextBox box = new NumberBox(NumberBox.NumberMode.Float, delegate (string val) { SetValue(idx, val); });
                box.Text = curve[i].ToString();
                box.Location = new Point(m_textboxX, m_TextboxCurY);

                m_LabelCurY += 30;
                m_TextboxCurY += 30;

                panel.Controls.Add(label);
                panel.Controls.Add(box);

                m_textBoxes[i] = box;
                m_labels[i] = label;
            }

            ResumeLayout(true);

            FormClosed += delegate { OnClose(); };
        }

        private void OnClose()
        {
            Form1.Instance.Enabled = true;
            m_finishedCallback?.Invoke(m_curve);
            m_finishedCallback_float?.Invoke(m_float_curve);
        }

        private void SetValue(int index, string val)
        {
            if (m_curve != null)
                m_curve[index] = (byte)(Utils.InvariantParse(val) * 255);
            else
                m_float_curve[index] = Utils.InvariantParse(val);
        }

        private void SetRange(int start, int end, float value)
        {
            if (m_curve != null)
            {
                start = Math.Clamp(start, 0, m_curve.Length - 1);
                end = Math.Clamp(end, 0, m_curve.Length - 1);
                value = Math.Clamp(value, 0, 1);
            }
            else
            {
                start = Math.Clamp(start, 0, m_float_curve.Length - 1);
                end = Math.Clamp(end, 0, m_float_curve.Length - 1);
                value = Math.Clamp(value, 0, 1);
            }

            byte conv = (byte)(value * 255f);

            try
            {
                for (int i = start; i < end + 1; i++)
                {
                    if (m_curve != null)
                        m_curve[i] = conv;
                    else
                        m_float_curve[i] = value;
                    m_textBoxes[i].Text = value.ToString();
                }
            }
            catch
            {
                startRangeBox.Text = "0";
                endRangeBox.Text = "0";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SetRange(int.Parse(startRangeBox.Text), int.Parse(endRangeBox.Text), Utils.InvariantParse(customValueBox.Text));
            }
            catch
            {
                customValueBox.Text = "0";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetRange(int.Parse(startRangeBox.Text), int.Parse(endRangeBox.Text), 0.5f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetRange(int.Parse(startRangeBox.Text), int.Parse(endRangeBox.Text), 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetRange(int.Parse(startRangeBox.Text), int.Parse(endRangeBox.Text), 1f);
        }
    }
}
