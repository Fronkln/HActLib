using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementSpeedControlWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementSpeedControl control = node as DEElementSpeedControl;

            PropertyEditorBuilder.CreateHeader("Speed Control");

            PropertyEditorBuilder.CreateInput("Speed Type", control.Type.ToString(), delegate (string val) { control.Type = (SpeedType)uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Minimum Speed", control.MinSpeed.ToString(), delegate (string val) { control.MinSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Maximum Speed", control.MaxSpeed.ToString(), delegate (string val) { control.MaxSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Frame Number", control.FrameNum.ToString(), delegate (string val) { control.FrameNum = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Is Use Unprocess", Convert.ToInt32(control.IsUseUnprocess).ToString(), delegate (string val) { control.IsUseUnprocess = val == "1"; }, NumberBox.NumberMode.Byte);

            PropertyEditorBuilder.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(control.AnimData,
                    delegate (float[] outCurve)
                    {
                       control.AnimData = outCurve;
                    });
            });
        }
    }
}
