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

            PropertyEditorBuilder.CreateHeader("Color Correction Mask");
            PropertyEditorBuilder.CreateInput("Hue", mask.Hue.ToString(), delegate (string val) { mask.Hue = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Saturation", mask.Saturation.ToString(), delegate (string val) { mask.Saturation = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Contrast", mask.Contrast.ToString(), delegate (string val) { mask.Contrast = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Luminance", mask.Luminance.ToString(), delegate (string val) { mask.Luminance = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Disable Character", Convert.ToInt32(mask.DisableCharacter).ToString(), delegate (string val) { mask.DisableCharacter = val.Equals("1"); });
            PropertyEditorBuilder.CreateButton("Curve", delegate
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
