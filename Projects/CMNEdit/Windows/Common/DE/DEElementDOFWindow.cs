using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit.Windows
{
    internal static class DEElementDOFWindow
    {
        public static void Draw(Form1 form, Node element)
        {
            DEElementDOF dof = element as DEElementDOF;

            form.CreateHeader("DOF");
            form.CreateInput("Disable", Convert.ToInt32(dof.DisableDof).ToString(), delegate (string val) { dof.DisableDof = int.Parse(val) > 0; });
            form.CreateInput("Use Intr", Convert.ToInt32(dof.UseIntr).ToString(), delegate (string val) { dof.UseIntr = int.Parse(val) > 0; });

            form.CreateInput("Shape", dof.Shape.ToString(), delegate (string val) { dof.Shape = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Diapharagm Blades", dof.DiaphragmBladesNum.ToString(), delegate (string val) { dof.DiaphragmBladesNum = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Aperture Circularity", dof.ApertureCircularity.ToString(), delegate (string val) { dof.ApertureCircularity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Focus Dist Before", dof.FocusDistBefore.ToString(), delegate (string val) { dof.FocusDistBefore = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Focus Dist After", dof.FocusDistAfter.ToString(), delegate (string val) { dof.FocusDistAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Edge Type", Convert.ToInt32(dof.EdgeType).ToString(), delegate (string val) { dof.EdgeType = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Focus Threshold", dof.EdgeThreshold.ToString(), delegate (string val) { dof.EdgeThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Quality", Convert.ToInt32(dof.Quality).ToString(), delegate (string val) { dof.Quality = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Focus Local Pos", Convert.ToInt32(dof.FocusLocalPos).ToString(), delegate (string val) { dof.FocusLocalPos = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Alpha To Coverage Depth Threshold", dof.AlphaToCoverageDepthThreshold.ToString(), delegate (string val) { dof.AlphaToCoverageDepthThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Lens Type", dof.LensType.ToString(), delegate (string val) { dof.LensType = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Aberration", dof.Aberration.ToString(), delegate (string val) { dof.Aberration = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Aberration FOV", dof.AberrationFOV.ToString(), delegate (string val) { dof.AberrationFOV = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Gradient Threshold", dof.GradientThreshold.ToString(), delegate (string val) { dof.GradientThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Gradient Min Threshold", dof.GradientMinThreshold.ToString(), delegate (string val) { dof.GradientMinThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Gradient Max Threshold", dof.GradientMaxThreshold.ToString(), delegate (string val) { dof.GradientMaxThreshold = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Near Focus Distance", dof.NearFocusDistance.ToString(), delegate (string val) { dof.NearFocusDistance = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("DOF After Disable Dist", dof.DOFAfterDisableDist.ToString(), delegate (string val) { dof.DOFAfterDisableDist = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
