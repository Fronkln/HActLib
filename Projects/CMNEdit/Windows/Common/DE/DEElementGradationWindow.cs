using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using System.Drawing;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class DEElementGradationWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementGradation grad = node as DEElementGradation;

            form.CreateHeader("Gradation (Incomplete)");

            form.CreateInput("Depth", grad.Depth.ToString(), delegate(string val) { grad.Depth = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Deep", grad.Deep.ToString(), delegate (string val) { grad.Deep = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Power", grad.Power.ToString(), delegate (string val) { grad.Power = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(grad.Animation,
                    delegate (byte[] outCurve)
                    {
                        grad.Animation = outCurve;
                    });
            });

            Panel color1Panel = null;
            Panel color1AfterPanel = null;

            color1Panel = form.CreatePanel("Color 1", grad.ColorL1,
                delegate (Color col)
                {
                    grad.ColorL1 = col;
                    color1Panel.BackColor = col;
                });

            color1AfterPanel = form.CreatePanel("Color 1 After", grad.ColorL1After,
                delegate (Color col)
                {
                    grad.ColorL1After = col;
                    color1AfterPanel.BackColor = col;
                });

            /*
            form.CreateButton("Color1", delegate
            {
                ColorDialog dig = new ColorDialog();
                dig.Color = Color.FromArgb((int)grad.ColorL1.R, (int)grad.ColorL1.G, (int)grad.ColorL1.B);
                dig.ShowDialog();
            });
            */
        }
    }
}
