using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementColorCorrectionMaskWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementColorCorrectionMask mask = node as DEElementColorCorrectionMask;

            form.CreateHeader("Color Correction Mask");
            form.CreateInput("Hue", mask.Hue.ToString(), delegate (string val) { mask.Hue = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Saturation", mask.Saturation.ToString(), delegate (string val) { mask.Saturation = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Contrast", mask.Contrast.ToString(), delegate (string val) { mask.Contrast = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Luminance", mask.Luminance.ToString(), delegate (string val) { mask.Luminance = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Disable Character", Convert.ToInt32(mask.DisableCharacter).ToString(), delegate (string val) { mask.DisableCharacter = val.Equals("1"); });
            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(mask.Animation,
                    delegate (byte[] outCurve)
                    {
                        mask.Animation = outCurve;
                    });
            });
        }
    }
}
