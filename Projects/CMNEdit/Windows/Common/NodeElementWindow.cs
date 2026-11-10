using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal class NodeElementWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeElement inf = node as NodeElement;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Element Info");

            PropertyEditorBuilder.CreateInput("ID", inf.ElementKind.ToString(), delegate (string val) { inf.ElementKind = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Version", inf.Version.ToString(), delegate(string val) { inf.Version = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Element Flag", inf.ElementFlag.ToString(), delegate(string val){ inf.ElementFlag = uint.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateComboBox("Play Type", (int)inf.PlayType, System.Enum.GetNames(typeof(ElementPlayType)), delegate (int val) { inf.PlayType = (ElementPlayType)val; });
            PropertyEditorBuilder.CreateInput("Update Timing Mode", inf.UpdateTimingMode.ToString(), delegate (string val) { inf.UpdateTimingMode = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            PropertyEditorBuilder.CreateInput("Start", inf.Start.ToString(), delegate(string val){ inf.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("End", inf.End.ToString(), delegate (string val) { inf.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
