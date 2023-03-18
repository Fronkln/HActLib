using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit.Windows;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementUIFadeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementUIFade uiFade = node as DEElementUIFade;

            form.CreateHeader("UI Fade");

            Panel colorPanel = null;

            form.CreateHeader("Fade");

            form.CreateInput("Flags", uiFade.Flags.ToString(), delegate (string val) { uiFade.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("In Frame", uiFade.InFrame.ToString(), delegate (string val) { uiFade.InFrame = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Out Frame", uiFade.OutFrame.ToString(), delegate (string val) { uiFade.OutFrame = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            colorPanel = form.CreatePanel("Fade Color", uiFade.Color,
                delegate (Color col)
                {
                    uiFade.Color = col;
                    colorPanel.BackColor = col;
                });

            form.CreateButton("Curve", delegate
            {
                CurveView curveForm = new CurveView();
                curveForm.Visible = true;
                curveForm.Init(uiFade.Animation,
                    delegate (float[] outCurve)
                    {
                        uiFade.Animation = outCurve;
                    });
            });
        }
    }
}
