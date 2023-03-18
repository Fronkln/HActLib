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

            form.CreateSpace(25);
            form.CreateHeader("Character Out");

            form.CreateComboBox("Exit Mode", (int)inf.ReturnType, Enum.GetNames(typeof(AuthReturnType)), delegate (int id) { inf.ReturnType = (AuthReturnType)id; });
            form.CreateInput("Play Range", inf.PlayRange.Frame.ToString(), delegate (string val) { inf.PlayRange.Frame = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Tick Length", inf.TickLength.ToString(), delegate (string val) { inf.TickLength = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Ragdoll", Convert.ToInt32(inf.RagdollInfoExists).ToString(), delegate (string val) { inf.RagdollInfoExists = uint.Parse(val) == 1; }, NumberBox.NumberMode.UInt);

            //TimingInfoRagdollWindow.Draw(form, inf.RagdollInfo);
        }
    }
}
