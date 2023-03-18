using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementColorCorrectionWindow
    {
        public static void Draw(Form1 form, Node colorCorrect)
        {
            DEElementColorCorrection correction = colorCorrect as DEElementColorCorrection;

            form.CreateHeader("Color Correction");
            form.CreateInput("Interpolation", correction.Interpolation.ToString(), delegate (string val) { correction.Interpolation = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
