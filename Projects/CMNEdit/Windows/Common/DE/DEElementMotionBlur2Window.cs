using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementMotionBlur2Window
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementMotionBlur2 mb2 = node as DEElementMotionBlur2;

            form.CreateHeader("Motion Blur");
            form.CreateInput("Shutter Speed", mb2.ShutterSpeed.ToString(), delegate (string val) { mb2.ShutterSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Blur Length", mb2.BlurLength.ToString(), delegate (string val) { mb2.BlurLength = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Sample Count", mb2.SampleCount.ToString(), delegate (string val) { mb2.SampleCount = int.Parse(val); }, NumberBox.NumberMode.Int);

            if(Form1.curGame >= Game.LADIW)
                form.CreateInput("Unknown", mb2.Unknown.ToString(), delegate (string val) { mb2.Unknown = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
