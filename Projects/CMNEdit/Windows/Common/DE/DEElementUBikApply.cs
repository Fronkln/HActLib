using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementUBikApplyWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementUBIKApply ubik = (DEElementUBIKApply)node;

            PropertyEditorBuilder.CreateHeader("Apply UBIK");
            PropertyEditorBuilder.CreateInput("Ubik", ubik.Ubik, delegate(string val) { ubik.Ubik = val; });
            PropertyEditorBuilder.CreateInput("Unknown", ubik.Unknown1.ToString(), delegate (string val) { ubik.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Unknown", ubik.Unknown2.ToString(), delegate (string val) { ubik.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
