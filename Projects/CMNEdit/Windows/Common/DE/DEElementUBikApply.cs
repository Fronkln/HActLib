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

            form.CreateHeader("Apply UBIK");
            form.CreateInput("Ubik", ubik.Ubik, delegate(string val) { ubik.Ubik = val; });
            form.CreateInput("Unknown", ubik.Unknown1.ToString(), delegate (string val) { ubik.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", ubik.Unknown2.ToString(), delegate (string val) { ubik.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
