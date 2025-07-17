using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class CSVRootWindow
    {
        public static void Draw(Form1 form)
        {
            form.CreateHeader($"HAct {Form1.TevCsvEntry.ID}", 0, true);
            form.CreateInput("Name", Form1.TevCsvEntry.Name, delegate(string val) { Form1.TevCsvEntry.Name = val; }, isCsvTree:true);
            form.CreateInput("Unknown", Form1.TevCsvEntry.Unk1.ToString(), delegate (string val) { Form1.TevCsvEntry.Unk1 = int.Parse(val); }, isCsvTree: true);
            form.CreateInput("Unknown", Form1.TevCsvEntry.Unk2.ToString(), delegate (string val) { Form1.TevCsvEntry.Unk2 = int.Parse(val); }, isCsvTree: true);
            form.CreateInput("Flags", Form1.TevCsvEntry.Flags.ToString(), delegate (string val) { Form1.TevCsvEntry.Flags = int.Parse(val); }, isCsvTree: true);
        }
    }
}
