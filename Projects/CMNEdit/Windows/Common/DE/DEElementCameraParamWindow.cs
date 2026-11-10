using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementCameraParamWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCameraParam paramNode = node as DEElementCameraParam;

            PropertyEditorBuilder.CreateHeader("Camera Param");

            PropertyEditorBuilder.CreateInput("Before Pos X", paramNode.BeforePos.x.ToString(), delegate (string val) { paramNode.BeforePos.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Pos Y", paramNode.BeforePos.y.ToString(), delegate (string val) { paramNode.BeforePos.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Pos Z", paramNode.BeforePos.z.ToString(), delegate (string val) { paramNode.BeforePos.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("Before Intr X", paramNode.BeforeIntr.x.ToString(), delegate (string val) { paramNode.BeforeIntr.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Intr Y", paramNode.BeforeIntr.y.ToString(), delegate (string val) { paramNode.BeforeIntr.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Intr Z", paramNode.BeforeIntr.z.ToString(), delegate (string val) { paramNode.BeforeIntr.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Before Fov Y", paramNode.BeforeFovY.ToString(), delegate (string val) { paramNode.BeforeFovY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("Before Up X", paramNode.BeforeUp.x.ToString(), delegate (string val) { paramNode.BeforeUp.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Up Y", paramNode.BeforeUp.y.ToString(), delegate (string val) { paramNode.BeforeUp.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Before Up Z", paramNode.BeforeUp.z.ToString(), delegate (string val) { paramNode.BeforeUp.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("After Pos X", paramNode.AfterPos.x.ToString(), delegate (string val) { paramNode.AfterPos.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("After Pos Y", paramNode.AfterPos.y.ToString(), delegate (string val) { paramNode.AfterPos.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("After Pos Z", paramNode.AfterPos.z.ToString(), delegate (string val) { paramNode.AfterPos.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateSpace(25);

            PropertyEditorBuilder.CreateInput("After Intr X", paramNode.AfterIntr.x.ToString(), delegate (string val) { paramNode.AfterIntr.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("After Intr Y", paramNode.AfterIntr.y.ToString(), delegate (string val) { paramNode.AfterIntr.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("After Intr Z", paramNode.AfterIntr.z.ToString(), delegate (string val) { paramNode.AfterIntr.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(paramNode.Animation,
                    delegate (float[] outCurve)
                    {
                        paramNode.Animation = outCurve;
                    });
            });
        }
    }
}
