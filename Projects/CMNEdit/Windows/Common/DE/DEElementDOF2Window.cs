using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit.Windows
{
    internal static  class DEElementDOF2Window
    {
        public static void Draw(Form1 form, Node element)
        {
            DEElementDOF2 dof2 = element as DEElementDOF2;

            form.CreateHeader("DOF2");
            form.CreateInput("Disable", Convert.ToInt32(dof2.DisableDof).ToString(), delegate(string val) { dof2.DisableDof = int.Parse(val)> 0 ; });

            form.CreateInput("Focus Dist Before", dof2.FocusDistBefore.ToString(), delegate (string val) { dof2.FocusDistBefore = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Focus Dist After", dof2.FocusDistAfter.ToString(), delegate (string val) { dof2.FocusDistAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Use Intr", Convert.ToInt32(dof2.UseIntr).ToString(), delegate (string val) { dof2.UseIntr = int.Parse(val) > 0; });

            form.CreateInput("Far Coc Rate", dof2.FarCocRate.ToString(), delegate (string val) { dof2.FarCocRate = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Near Coc Rate", dof2.NearCocRate.ToString(), delegate (string val) { dof2.NearCocRate = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Focus Far D", dof2.FocusFarD.ToString(), delegate (string val) { dof2.FocusFarD = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Far Max Coc Dist", dof2.FarMaxCocDist.ToString(), delegate (string val) { dof2.FarMaxCocDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Focus Near D", dof2.FocusNearD.ToString(), delegate (string val) { dof2.FocusNearD = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Near Max Coc Dist", dof2.NearMaxCocDist.ToString(), delegate (string val) { dof2.NearMaxCocDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Max Coc Radius", dof2.MaxCocRadius.ToString(), delegate (string val) { dof2.MaxCocRadius = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Specular Brightness", dof2.SpecularBrightness.ToString(), delegate (string val) { dof2.SpecularBrightness = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Specular Threshold", dof2.SpecularThresold.ToString(), delegate (string val) { dof2.SpecularThresold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Alpha", dof2.Alpha.ToString(), delegate (string val) { dof2.Alpha = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Color Scale", dof2.ColorScale.ToString(), delegate (string val) { dof2.ColorScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Shape", dof2.Shape.ToString(), delegate (string val) { dof2.Shape = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Diapharagm Blades", dof2.DiaphragmBladesNum.ToString(), delegate (string val) { dof2.DiaphragmBladesNum = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Shape Rot Angle", dof2.ShapeRotAngle.ToString(), delegate (string val) { dof2.ShapeRotAngle = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Aperture Circularity", dof2.ApertureCircularity.ToString(), delegate (string val) { dof2.ApertureCircularity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Bokeh Attenuation Begin Rate", dof2.BokehAttenBeginRate.ToString(), delegate (string val) { dof2.BokehAttenBeginRate = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("High Lumi Emphasis Thresold", dof2.HighLumiEmphasisThresold.ToString(), delegate (string val) { dof2.HighLumiEmphasisThresold= Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("High Lumi Emphasis Color", dof2.HighLumiEmphasisColor.ToString(), delegate (string val) { dof2.HighLumiEmphasisColor = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("High Lumi Emphasis Scale", dof2.HighLumiEmphasisScale.ToString(), delegate (string val) { dof2.HighLumiEmphasisScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Full Resolution Bokeh", Convert.ToInt32(dof2.FullResoBokeh).ToString(), delegate (string val) { dof2.FullResoBokeh = int.Parse(val) > 0; });
            form.CreateInput("Near Focus Distance", dof2.NearFocusDistance.ToString(), delegate (string val) { dof2.NearFocusDistance  = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("F Number", dof2.FNumber.ToString(), delegate (string val) { dof2.FNumber = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Use F Number", Convert.ToInt32(dof2.UseFNumber).ToString(), delegate (string val) { dof2.UseFNumber  = int.Parse(val) > 0; });
            form.CreateInput("Enable Focus Adjust", Convert.ToInt32(dof2.EnableFocusAdjust).ToString(), delegate (string val) { dof2.EnableFocusAdjust = int.Parse(val) > 0; });
            form.CreateInput("Focus Near DF Number", dof2.FocusNearDFNumber.ToString(), delegate (string val) { dof2.FocusNearDFNumber = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Focus Far DF Number", dof2.FocusFarDFNumber.ToString(), delegate (string val) { dof2.FocusFarDFNumber = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Calculate Before TAA", Convert.ToInt32(dof2.CalcBeforeTAA).ToString(), delegate (string val) { dof2.CalcBeforeTAA = int.Parse(val) > 0; });
            form.CreateInput("Max Coc Radius F", dof2.MaxCocRadiusF.ToString(), delegate (string val) { dof2.MaxCocRadiusF = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Cinematic DOF Quality Level", dof2.CinematicDofQualityLevel.ToString(), delegate (string val) { dof2.CinematicDofQualityLevel = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Enable Mirror", Convert.ToInt32(dof2.EnableMirror).ToString(), delegate (string val) { dof2.EnableMirror = int.Parse(val) > 0; });
            form.CreateInput("Enable Mirror Only", Convert.ToInt32(dof2.EnableMirrorOnly).ToString(), delegate (string val) { dof2.EnableMirrorOnly = int.Parse(val) > 0; });
            form.CreateInput("Scatter Mode", dof2.ScatterMode.ToString(), delegate (string val) { dof2.ScatterMode = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Ring Count", dof2.RingCount.ToString(), delegate (string val) { dof2.RingCount = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("TAA Quality", dof2.TAAQuality.ToString(), delegate (string val) { dof2.TAAQuality = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Recombine Quality", dof2.RecombineQuality.ToString(), delegate (string val) { dof2.RecombineQuality = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
