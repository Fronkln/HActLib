using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementMotionBlurWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementMotionBlur mb = node as DEElementMotionBlur;

            form.CreateHeader("Motion Blur");
            form.CreateInput("Shutter Speed", mb.ShutterSpeed.ToString(), delegate (string val) { mb.ShutterSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Blur Length", mb.BlurLength.ToString(), delegate (string val) { mb.BlurLength = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Sampling Interleave", mb.SamplingInterleave.ToString(), delegate (string val) { mb.SamplingInterleave = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Falloff Start Distance", mb.FalloffStartDist.ToString(), delegate (string val) { mb.FalloffStartDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Falloff End Distance", mb.FalloffEndDist.ToString(), delegate (string val) { mb.FalloffStartDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Rotation Perspective Threshold", mb.RotationPerspectiveThreshold.ToString(), delegate (string val) { mb.RotationPerspectiveThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trans Threshold", mb.TransThreshold.ToString(), delegate (string val) { mb.TransThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

        }
    }
}
