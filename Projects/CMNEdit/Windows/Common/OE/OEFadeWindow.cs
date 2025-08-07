using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using CMNEdit.Windows;

namespace CMNEdit
{
    internal static class OEFadeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEFade fade = node as OEFade;

            Panel colorPanel = null;

            form.CreateHeader("Fade");
            colorPanel = form.CreatePanel("Fade Color", fade.Color,
                delegate (Color col)
                {
                    fade.Color = col;
                    colorPanel.BackColor = col;
                });

            form.CreateButton("Curve", delegate
            {
                CurveView curveForm = new CurveView();
                curveForm.Visible = true;
                curveForm.Init(fade.Animation,
                    delegate (byte[] outCurve)
                    {
                        fade.Animation = outCurve;
                    });
            });
        }
    }
}
