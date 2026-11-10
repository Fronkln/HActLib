using System;

using HActLib;

namespace CMNEdit.Windows
{
    internal static class MatrixWindow
    {
        public static void Draw(Form1 form, Matrix4x4 mtx)
        {
            PropertyEditorBuilder.CreateInput("Left Direction X", mtx.VM0.x.ToString(), delegate (string val) { mtx.VM0.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Left Direction Y", mtx.VM0.y.ToString(), delegate (string val) { mtx.VM0.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Left Direction Z", mtx.VM0.z.ToString(), delegate (string val) { mtx.VM0.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Left Direction W", mtx.VM0.w.ToString(), delegate (string val) { mtx.VM0.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("Up Direction X", mtx.VM1.x.ToString(), delegate (string val) { mtx.VM1.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Up Direction Y", mtx.VM1.y.ToString(), delegate (string val) { mtx.VM1.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Up Direction Z", mtx.VM1.z.ToString(), delegate (string val) { mtx.VM1.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Up Direction W", mtx.VM1.w.ToString(), delegate (string val) { mtx.VM1.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("Forward Direction X", mtx.VM2.x.ToString(), delegate (string val) { mtx.VM2.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Forward Direction Y", mtx.VM2.y.ToString(), delegate (string val) { mtx.VM2.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Forward Direction Z", mtx.VM2.z.ToString(), delegate (string val) { mtx.VM2.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Forward Direction W", mtx.VM2.w.ToString(), delegate (string val) { mtx.VM2.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("Coordinate X (Left)", mtx.VM3.x.ToString(), delegate (string val) { mtx.VM3.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Coordinate Y (Up)", mtx.VM3.y.ToString(), delegate (string val) { mtx.VM3.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Coordinate Z (Forward)", mtx.VM3.z.ToString(), delegate (string val) { mtx.VM3.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Unused", mtx.VM3.w.ToString(), delegate (string val) { mtx.VM3.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
