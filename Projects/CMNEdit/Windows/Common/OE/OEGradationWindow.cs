using System;
using System.Drawing;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class OEGradationWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEGradation gradation = node as OEGradation;

            form.CreateHeader("Gradation");

            form.CreateInput("Unknown Color Related?", gradation.Unknown1.ToString(), delegate (string val) { gradation.Unknown1 = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown Color Related?", gradation.Unknown2.ToString(), delegate (string val) { gradation.Unknown2 = uint.Parse(val); }, NumberBox.NumberMode.UInt);


            Panel color1Panel = null;
            Panel color2Panel = null;
            Panel unkCol1Panel = null;
            Panel unkCol2Panel = null;

            color1Panel = form.CreatePanel("Color 1", gradation.Color1,
                delegate (Color col)
                {
                    gradation.Color1 = col;
                    color1Panel.BackColor = col;
                });

            form.CreateInput("Unknown Color Related?", gradation.Unknown3.ToString(), delegate (string val) { gradation.Unknown3 = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            color2Panel = form.CreatePanel("Color 2", gradation.Color2,
                delegate (Color col)
                {
                    gradation.Color2 = col;
                    color2Panel.BackColor = col;
                });

            form.CreateInput("Rotation (Degrees)", gradation.Rotation.ToString(), delegate (string val) { gradation.Rotation = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Color 1 Width", gradation.Color1Width.ToString(), delegate (string val) { gradation.Color1Width = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Color 2 Width", gradation.Color2Width.ToString(), delegate (string val) { gradation.Color2Width = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            unkCol1Panel = form.CreatePanel("Unknown Color Related?", gradation.UnknownColor1,
                delegate (Color col)
                {
                    gradation.UnknownColor1 = col;
                    unkCol1Panel.BackColor = col;
                });

            unkCol2Panel = form.CreatePanel("Unknown Color Related?", gradation.UnknownColor2,
                delegate (Color col)
                {
                    gradation.UnknownColor2 = col;
                    unkCol2Panel.BackColor = col;
                });

            form.CreateInput("Unknown", gradation.Unknown4.ToString(), delegate (string val) { gradation.Unknown4 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", gradation.Unknown5.ToString(), delegate (string val) { gradation.Unknown5 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
