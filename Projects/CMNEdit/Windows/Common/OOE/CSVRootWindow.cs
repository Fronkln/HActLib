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
            PropertyEditorBuilder.CreateHeader($"HAct {Form1.TevCsvEntry.ID}", 0);
            PropertyEditorBuilder.CreateInput("Name", Form1.TevCsvEntry.Name, delegate(string val) { Form1.TevCsvEntry.Name = val; });
            PropertyEditorBuilder.CreateInput("Unknown", Form1.TevCsvEntry.Unk1.ToString(), delegate (string val) { Form1.TevCsvEntry.Unk1 = int.Parse(val); });
            PropertyEditorBuilder.CreateInput("Unknown", Form1.TevCsvEntry.Unk2.ToString(), delegate (string val) { Form1.TevCsvEntry.Unk2 = int.Parse(val); });
            PropertyEditorBuilder.CreateInput("Flags", Form1.TevCsvEntry.Flags.ToString(), delegate (string val) { Form1.TevCsvEntry.Flags = int.Parse(val); });
        }
    }
}
