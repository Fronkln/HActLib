using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEHeatChangeWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHeat heat = node as OEHeat;

            form.CreateHeader("Heat Change");
            form.CreateInput("Heat Change", heat.HeatChange.ToString(), delegate (string val) { heat.HeatChange = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", heat.Unknown1.ToString(), delegate (string val) { heat.Unknown1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Unknown", heat.Unknown2.ToString(), delegate (string val) { heat.Unknown2 = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
