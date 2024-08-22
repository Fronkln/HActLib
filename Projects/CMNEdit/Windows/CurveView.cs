using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
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

        private static float[] copiedCurve = new float[0];

        DataPoint curPoint = null;
        int curPointIdx = 0;

        bool byteMode = false;

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

            m_float_curve = new float[curve.Length];

            for (int i = 0; i < m_float_curve.Length; i++)
            {
                m_float_curve[i] = (curve[i] / 255f);
            }

            for (int i = 0; i < curve.Length; i++)
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

            byteMode = true;

            ResumeLayout(true);
            FormClosed += delegate { OnClose(); };

            InitFinish();
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

            InitFinish();
        }

        private void InitFinish()
        {
            var objChart = chart1.ChartAreas[0];
            objChart.AxisX.Minimum = 0.0f;
            objChart.AxisX.Maximum = m_float_curve.Length - 1;
            objChart.AxisY.Minimum = 0f;
            objChart.AxisY.Maximum = 1f;
            objChart.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;

            chart1.Series.Clear();
            chart1.Series.Add("Value");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series[0].MarkerSize = 8;

            for (int i = 0; i < m_float_curve.Length; i++)
            {
                chart1.Series[0].Points.AddXY(i, m_float_curve[i]);
            }
        }

        private void OnClose()
        {
            Form1.Instance.Enabled = true;

            if (byteMode)
            {
                byte[] byteArray = new byte[m_float_curve.Length];

                for (int i = 0; i < byteArray.Length; i++)
                    byteArray[i] = (byte)(m_float_curve[i] * 255);

                m_finishedCallback?.Invoke(byteArray);
            }
            else
            {
                m_finishedCallback_float?.Invoke(m_float_curve);
            }
        }

        private void SetValue(int index, string val)
        {
            if (byteMode)
                m_curve[index] = (byte)(Utils.InvariantParse(val) * 255);
            else
                m_float_curve[index] = Utils.InvariantParse(val);

            InitFinish();
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

        RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }

        RectangleF InnerPlotPositionClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            curPoint = null;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                ChartArea ca = chart1.ChartAreas[0];
                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;

                //  RectangleF ippRect = InnerPlotPositionClientRectangle(chart1, ca);
                //   if (!ippRect.Contains(e.Location)) return;

                int yPos = e.Y;

                if (yPos < 0)
                    yPos = 0;

                System.Diagnostics.Debug.WriteLine(yPos);

                HitTestResult hit = chart1.HitTest(e.X, yPos);

                if (hit == null) return;
                if (hit.PointIndex >= 0 && curPoint == null)
                {
                    curPoint = hit.Series.Points[hit.PointIndex];
                    curPointIdx = hit.PointIndex;
                }

                if (curPoint != null)
                {
                    Series s = hit.Series;
                    double dy = 0;

                    try
                    {
                        dy = ay.PixelPositionToValue(yPos);
                    }

                    catch
                    {
                        dy = 0;
                    }


                    if (dy > ca.AxisY.Maximum)
                        dy = ca.AxisY.Maximum;
                    if (dy < ca.AxisY.Minimum)
                        dy = ca.AxisY.Minimum;

                    curPoint.XValue = curPoint.XValue;
                    curPoint.YValues[0] = dy;

                    m_textBoxes[curPointIdx].Text = dy.ToString();
                    m_float_curve[curPointIdx] = (float)dy;
                }
            }
        }

        private void copyCurveButton_Click(object sender, EventArgs e)
        {
            if (m_float_curve.Length <= 0)
                return;

            copiedCurve = m_float_curve;
        }

        private void pasteCurveButton_Click(object sender, EventArgs e)
        {
            if (m_float_curve.Length != copiedCurve.Length)
                return;

            m_float_curve = copiedCurve;
            InitFinish();
        }
    }
}
