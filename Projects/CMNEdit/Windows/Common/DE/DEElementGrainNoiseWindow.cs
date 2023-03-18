using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit.Windows;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementGrainNoiseWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementGrainNoise noise = node as DEElementGrainNoise;

            form.CreateHeader("Grain Noise");

            form.CreateInput("Power", noise.Power.ToString(), delegate (string val) { noise.Power = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Size", noise.Size.ToString(), delegate (string val) { noise.Size = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Intensity", noise.Intensity.ToString(), delegate (string val) { noise.Intensity = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            form.CreateButton("Curve", delegate
            {
                CurveView curveForm = new CurveView();
                curveForm.Visible = true;
                curveForm.Init(noise.Animation,
                    delegate (byte[] outCurve)
                    {
                        noise.Animation = outCurve;
                    });
            });
        }
    }
}
