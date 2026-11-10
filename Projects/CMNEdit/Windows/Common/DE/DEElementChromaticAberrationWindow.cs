using System;
using System.Globalization;
using CMNEdit.Windows;
using HActLib;
using ParLibrary;

namespace CMNEdit
{
    internal static class DEElementChromaticAberrationWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementChromaticAberration chromatic = node as DEElementChromaticAberration;

            PropertyEditorBuilder.CreateHeader("Chromatic Aberration");

            PropertyEditorBuilder.CreateInput("Samples", chromatic.Samples.ToString(), delegate (string val) { chromatic.Samples = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Magnification Dispersion", chromatic.MagnificationDispersion.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.MagnificationDispersion = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Uniform Dispersion X", chromatic.UniformDispersionX.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Uniform Dispersion Y", chromatic.UniformDispersionX.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Magnification Dispersion After", chromatic.MagnificationDispersionAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.MagnificationDispersionAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Uniform Dispersion X After", chromatic.UniformDispersionXAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionXAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Uniform Dispersion Y After", chromatic.UniformDispersionXAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionYAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateButton("Curve", delegate
            {
                CurveView myNewForm = new CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(chromatic.Animation,
                    delegate (float[] outCurve)
                    {
                        chromatic.Animation = outCurve;
                    });
            });
        }
    }
}
