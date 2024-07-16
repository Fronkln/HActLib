using System;

using HActLib;

namespace CMNEdit.Windows
{
    internal static class MatrixWindow
    {
        public static void Draw(Form1 form, Matrix4x4 mtx)
        {
            form.CreateInput("Left Direction X", mtx.VM0.x.ToString(), delegate (string val) { mtx.VM0.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Left Direction Y", mtx.VM0.y.ToString(), delegate (string val) { mtx.VM0.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Left Direction Z", mtx.VM0.z.ToString(), delegate (string val) { mtx.VM0.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Left Direction W", mtx.VM0.w.ToString(), delegate (string val) { mtx.VM0.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateSpace(25);

            form.CreateInput("Up Direction X", mtx.VM1.x.ToString(), delegate (string val) { mtx.VM1.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Up Direction Y", mtx.VM1.y.ToString(), delegate (string val) { mtx.VM1.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Up Direction Z", mtx.VM1.z.ToString(), delegate (string val) { mtx.VM1.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Up Direction W", mtx.VM1.w.ToString(), delegate (string val) { mtx.VM1.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateSpace(25);

            form.CreateInput("Forward Direction X", mtx.VM2.x.ToString(), delegate (string val) { mtx.VM2.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Forward Direction Y", mtx.VM2.y.ToString(), delegate (string val) { mtx.VM2.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Forward Direction Z", mtx.VM2.z.ToString(), delegate (string val) { mtx.VM2.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Forward Direction W", mtx.VM2.w.ToString(), delegate (string val) { mtx.VM2.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateSpace(25);

            form.CreateInput("Coordinate X (Left)", mtx.VM3.x.ToString(), delegate (string val) { mtx.VM3.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Coordinate Y (Up)", mtx.VM3.y.ToString(), delegate (string val) { mtx.VM3.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Coordinate Z (Forward)", mtx.VM3.z.ToString(), delegate (string val) { mtx.VM3.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unused", mtx.VM3.w.ToString(), delegate (string val) { mtx.VM3.w = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
