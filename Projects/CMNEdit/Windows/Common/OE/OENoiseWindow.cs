using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OENoiseWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OENoise noise = node as OENoise;

            PropertyEditorBuilder.CreateHeader("Noise");
            PropertyEditorBuilder.CreateInput("Power", noise.Power.ToString(), delegate (string val) { noise.Power = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Size", noise.Size.ToString(), delegate (string val) { noise.Size = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Unknown", noise.NoiseUnk3.ToString(), delegate (string val) { noise.NoiseUnk3 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Intensity", noise.Power.ToString(), delegate (string val) { noise.Power = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(noise.Animation,
                    delegate (byte[] outCurve)
                    {
                        noise.Animation = outCurve;
                    });
            });
        }
    }
}
