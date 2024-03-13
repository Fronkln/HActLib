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
            form.CreateHeader("Basic Information");

            bool bep = Form1.IsBep;

            if (bep)
            {
                form.CreateInput("Parent/Identifier", node.Guid.ToString(), delegate (string val) { node.Guid = new Guid(val); Form1.Instance.ProcessBEPHierarchy(); });
                form.CreateInput("GUID", node.BEPDat.Guid2.ToString(), delegate(string val) { node.BEPDat.Guid2 = new Guid(val); });
                form.CreateInput("Bone", node.BEPDat.Bone.Text, delegate(string val) { node.BEPDat.Bone.Set(val); });
            }
            else
            {
                form.CreateInput("GUID", node.Guid.ToString(), delegate (string val) { node.Guid = new Guid(val); });
            }


            form.CreateInput("Name", node.Name.ToString(),
                 delegate (string val)
                 {
                     Form1.EditingNode.Text = val;
                     node.Name = val;
                 }, NumberBox.NumberMode.Text, bep);

            form.CreateInput("Unknown", node.BEPDat.DataUnk.ToString(), delegate (string val) { node.BEPDat.DataUnk = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Category", node.Category.ToString(), null, readOnly: true);
            form.CreateInput("Flags", node.Flag.ToString(), delegate(string val) { node.Flag = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Priority", node.Priority.ToString(), delegate(string val) { node.Priority = int.Parse(val); }, NumberBox.NumberMode.Int);

            if (bep)
                form.CreateInput("Unknown", node.BEPDat.DataUnk.ToString(), delegate (string val) { node.BEPDat.DataUnk = int.Parse(val); }, NumberBox.NumberMode.Int);

            NodeResourceWindow.Draw(form, node);
        }
    }
}
