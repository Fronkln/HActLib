using HActLib;

namespace CMNEdit
{
    internal static class DEElementCameraLinkWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCameraLink link = node as DEElementCameraLink;

            form.CreateHeader("Camera Link");
            form.CreateInput("Flags", link.LinkFlags.ToString(), delegate (string val) { link.LinkFlags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Y Rotation Degree Offset", link.RotationYOffsetDegrees.ToString(), delegate (string val) { link.RotationYOffsetDegrees = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);


            form.CreateInput("Trs Pos X", link.TrsPos.x.ToString(), delegate (string val) { link.TrsPos.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Pos Y", link.TrsPos.y.ToString(), delegate (string val) { link.TrsPos.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Pos Z", link.TrsPos.z.ToString(), delegate (string val) { link.TrsPos.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Trs Intr X", link.TrsIntr.x.ToString(), delegate (string val) { link.TrsIntr.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Intr Y", link.TrsIntr.y.ToString(), delegate (string val) { link.TrsIntr.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Intr Z", link.TrsIntr.z.ToString(), delegate (string val) { link.TrsIntr.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Trs Up X", link.TrsUp.x.ToString(), delegate (string val) { link.TrsUp.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Up Y", link.TrsUp.y.ToString(), delegate (string val) { link.TrsUp.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Up Z", link.TrsUp.z.ToString(), delegate (string val) { link.TrsUp.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Trs Fov Y Radians", link.TrsFovYRadians.ToString(), delegate (string val) { link.TrsFovYRadians = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Trs Near Clip", link.TrsClipNear.ToString(), delegate (string val) { link.TrsClipNear = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Trs Far Clip", link.TrsClipFar.ToString(), delegate (string val) { link.TrsClipFar = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(link.Animation,
                    delegate (byte[] outCurve)
                    {
                        link.Animation = outCurve;
                    });
            });
        }
    }
}
