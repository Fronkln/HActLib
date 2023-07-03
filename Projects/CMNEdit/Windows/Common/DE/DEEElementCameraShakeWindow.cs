using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEEElementCameraShakeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCameraShake shake = node as DEElementCameraShake;

            form.CreateHeader("Camera Shake");
            form.CreateComboBox("Type", (int)shake.Type, Enum.GetNames<CameraShakeType>(), delegate (int idx) { shake.Type = (CameraShakeType)idx; });
            form.CreateInput("Scale", shake.Scale.ToString(), delegate (string val) { shake.Scale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Speed", shake.Speed.ToString(), delegate (string val) { shake.Speed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Priority Index", shake.PriorityID.ToString(), delegate (string val) { shake.PriorityID = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
