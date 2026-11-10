using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class TimingInfoRagdollWindow
    {
        public static void Draw(Form1 form, TimingInfoRagdoll ragdoll)
        {
            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Ragdoll Info");

            PropertyEditorBuilder.CreateInput("Parameter", ragdoll.Param.ToString(), delegate (string val) { ragdoll.Param = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Flag", ragdoll.Flag.ToString(), delegate (string val) { ragdoll.Flag = int.Parse(val); }, NumberBox.NumberMode.Int);

            PropertyEditorBuilder.CreateInput("X Velocity", ragdoll.VelocityX.ToString(), delegate (string val) { ragdoll.VelocityX = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Y Velocity", ragdoll.VelocityY.ToString(), delegate (string val) { ragdoll.VelocityY = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Z Velocity", ragdoll.VelocityZ.ToString(), delegate (string val) { ragdoll.VelocityZ = float.Parse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("X Angular Velocity", ragdoll.AngularX.ToString(), delegate (string val) { ragdoll.AngularX = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Y Angular Velocity", ragdoll.AngularY.ToString(), delegate (string val) { ragdoll.AngularY = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Z Angular Velocity", ragdoll.AngularZ.ToString(), delegate (string val) { ragdoll.AngularZ = float.Parse(val); }, NumberBox.NumberMode.Float);

        }
    }
}