using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEDamageWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEDamage inf = node as OEDamage;

            form.CreateSpace(25);
            form.CreateHeader("Damage");

            form.CreateInput("Damage",  inf.Damage.ToString(), delegate(string val) { inf.Damage = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", inf.Unk1.ToString(), delegate (string val) { inf.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Unknown", inf.Unk2.ToString(), delegate (string val) { inf.Unk2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Dont Kill", Convert.ToByte(inf.NoDead).ToString(), delegate (string val) { inf.NoDead = byte.Parse(val) == 1; }, NumberBox.NumberMode.Byte);
        }
    }
}
