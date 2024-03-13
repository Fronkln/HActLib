using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class HActEventDamageWindow
    {
        public static void Draw(Form1 form, CSVHActEventDamage damage, bool isCsvTree = false)
        {
            if (damage == null)
                return;

            form.CreateHeader("Damage (CSV)", isCsvTree: isCsvTree);

            form.CreateInput("Damage", damage.Damage.ToString(), delegate (string val) { damage.Damage = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown1.ToString(), delegate (string val) { damage.Unknown1 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown2.ToString(), delegate (string val) { damage.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown3.ToString(), delegate (string val) { damage.Unknown3 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown4.ToString(), delegate (string val) { damage.Unknown4 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown5.ToString(), delegate (string val) { damage.Unknown5 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
            form.CreateInput("Unknown", damage.Unknown6.ToString(), delegate (string val) { damage.Unknown6 = int.Parse(val); }, NumberBox.NumberMode.Int, isCsvTree: isCsvTree);
        }
    }
}
