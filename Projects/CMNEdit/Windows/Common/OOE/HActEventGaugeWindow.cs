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
        public static void Draw(Form1 form, CSVHActEventHeatGauge gauge, bool isCsvTree = false)
        {
            if (gauge == null)
                return;

            form.CreateHeader("Heat Change (CSV)", isCsvTree:isCsvTree);

            form.CreateInput("Change", gauge.Change.ToString(), delegate (string val) { gauge.Change = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
        }
    }
}
