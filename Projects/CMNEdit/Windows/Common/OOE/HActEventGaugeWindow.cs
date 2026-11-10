using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class HActEventGaugeWindow
    {
        public static void Draw(Form1 form, CSVHActEventHeatGauge gauge)
        {
            if (gauge == null)
                return;

            PropertyEditorBuilder.CreateHeader("Heat Change (CSV)");

            PropertyEditorBuilder.CreateInput("Change", gauge.Change.ToString(), delegate (string val) { gauge.Change = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
