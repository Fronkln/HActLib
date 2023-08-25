using HActLib;
using PIBLib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementColorCorrectionWindow
    {
        public static void Draw(Form1 form, Node colorCorrect)
        {
            DEElementColorCorrection correction = colorCorrect as DEElementColorCorrection;

            form.CreateHeader("Color Correction");


            form.CreateInput("Use Curve", Convert.ToByte(correction.UseCurve).ToString(), delegate (string val) { correction.UseCurve = byte.Parse(val) > 0; }, NumberBox.NumberMode.Byte);

            if (Form1.curVer > GameVersion.DE1)
            {
                form.CreateInput("Use Saturation", Convert.ToByte(correction.UseSaturation).ToString(), delegate (string val) { correction.UseSaturation = byte.Parse(val) > 0; }, NumberBox.NumberMode.Byte);

                form.CreateInput("Contrast", correction.Contrast.ToString(), delegate (string val) { correction.Contrast = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                form.CreateInput("Saturation Control", correction.SaturationControl.ToString(), delegate (string val) { correction.SaturationControl = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            }
            form.CreateButton("View", delegate
            {
                using (MemoryStream ms = new MemoryStream(correction.ToBitmapBytes()))
                {
                    Image lutBmp = Bitmap.FromStream(ms);

                    LutView view = new LutView();
                    view.pictureBox2.Image = lutBmp;
                    view.Show();
                }
            });

            form.CreateButton("Replace with BMP",
                delegate
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "BMP File | *.bmp";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
                        {
                            using (BinaryReader reader = new BinaryReader(ms))
                            {
                                byte[] buf = new byte[reader.BaseStream.Length - 54];
                                reader.BaseStream.Position += 54;
                                reader.BaseStream.Read(buf, 0, buf.Length);
                                correction.unkBytes = buf;
                            }
                        }
                    }
                });

            form.CreateButton("Export BMP", delegate
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "BMP File | *.bmp";


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    correction.ExportToBMP(saveFileDialog.FileName);
                }
            });

            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(correction.Animation,
                    delegate (float[] outCurve)
                    {
                        correction.Animation = outCurve;
                    });
            });
        }
    }
}
