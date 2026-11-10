using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEEElementCameraShakeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCameraShake shake = node as DEElementCameraShake;

            PropertyEditorBuilder.CreateHeader("Camera Shake");
            PropertyEditorBuilder.CreateComboBox("Type", (int)shake.Type, Enum.GetNames<CameraShakeType>(), delegate (int idx) { shake.Type = (CameraShakeType)idx; });
            PropertyEditorBuilder.CreateInput("Scale", shake.Scale.ToString(), delegate (string val) { shake.Scale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Speed", shake.Speed.ToString(), delegate (string val) { shake.Speed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Priority Index", shake.PriorityID.ToString(), delegate (string val) { shake.PriorityID = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
