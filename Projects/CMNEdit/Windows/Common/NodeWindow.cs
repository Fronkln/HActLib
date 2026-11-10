using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class NodeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            PropertyEditorBuilder.CreateHeader("Basic Information");

            bool bep = Form1.IsBep;

            if (bep)
            {
                PropertyEditorBuilder.CreateInput("Parent", node.Guid.ToString(), delegate (string val) { node.Guid = new Guid(val); Form1.Instance.ProcessBEPHierarchy(); });
                PropertyEditorBuilder.CreateInput("GUID", node.BEPDat.Guid2.ToString(), delegate(string val) { node.BEPDat.Guid2 = new Guid(val); });
                PropertyEditorBuilder.CreateInput("Bone", node.BEPDat.Bone.Text, delegate(string val) { node.BEPDat.Bone.Set(val); });
            }
            else
            {
                PropertyEditorBuilder.CreateInput("GUID", node.Guid.ToString(), delegate (string val) { node.Guid = new Guid(val); });
            }


            PropertyEditorBuilder.CreateInput("Name", node.Name.ToString(),
                 delegate (string val)
                 {
                     Form1.EditingNode.Text = val;
                     node.Name = val;
                 }, NumberBox.NumberMode.Text, bep);

            PropertyEditorBuilder.CreateInput("Unknown", node.BEPDat.DataUnk.ToString(), delegate (string val) { node.BEPDat.DataUnk = int.Parse(val); }, NumberBox.NumberMode.Int);

            PropertyEditorBuilder.CreateInput("Category", node.Category.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Flags", node.Flag.ToString(), delegate(string val) { node.Flag = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Priority", node.Priority.ToString(), delegate(string val) { node.Priority = int.Parse(val); }, NumberBox.NumberMode.Int);

            if (bep)
                PropertyEditorBuilder.CreateInput("Unknown", node.BEPDat.DataUnk.ToString(), delegate (string val) { node.BEPDat.DataUnk = int.Parse(val); }, NumberBox.NumberMode.Int);

            NodeResourceWindow.Draw(form, node);
        }
    }
}
