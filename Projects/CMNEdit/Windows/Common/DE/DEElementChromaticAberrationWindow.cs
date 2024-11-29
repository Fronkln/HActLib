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

            form.CreateHeader("Chromatic Aberration");

            form.CreateInput("Samples", chromatic.Samples.ToString(), delegate (string val) { chromatic.Samples = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Magnification Dispersion", chromatic.MagnificationDispersion.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.MagnificationDispersion = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Uniform Dispersion X", chromatic.UniformDispersionX.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Uniform Dispersion Y", chromatic.UniformDispersionX.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Magnification Dispersion After", chromatic.MagnificationDispersionAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.MagnificationDispersionAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Uniform Dispersion X After", chromatic.UniformDispersionXAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionXAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Uniform Dispersion Y After", chromatic.UniformDispersionXAfter.ToString(CultureInfo.InvariantCulture), delegate (string val) { chromatic.UniformDispersionYAfter = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateButton("Curve", delegate
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
