using System;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;
namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementCharaOutWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCharaOut inf = node as DEElementCharaOut;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Character Out");

            PropertyEditorBuilder.CreateComboBox("Exit Mode", (int)inf.ReturnType, Enum.GetNames(typeof(AuthReturnType)), delegate (int id) { inf.ReturnType = (AuthReturnType)id; });
            PropertyEditorBuilder.CreateInput("Play Range", inf.PlayRange.Frame.ToString(), delegate (string val) { inf.PlayRange.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Tick Length", inf.TickLength.ToString(), delegate (string val) { inf.TickLength = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Ragdoll", Convert.ToInt32(inf.RagdollInfoExists).ToString(), delegate (string val) { inf.RagdollInfoExists = uint.Parse(val) == 1; }, NumberBox.NumberMode.UInt);

            //TimingInfoRagdollWindow.Draw(form, inf.RagdollInfo);
        }
    }
}
