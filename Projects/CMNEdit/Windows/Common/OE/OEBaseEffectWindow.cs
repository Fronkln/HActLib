using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEBaseEffectWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEBaseEffect effect = node as OEBaseEffect;

            form.CreateHeader("Effect (Base)");
            form.CreateInput("Unknown", effect.Unk1.ToString(), delegate (string val) { effect.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", effect.Unk2.ToString(), delegate (string val) { effect.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", effect.Unk3.ToString(), delegate (string val) { effect.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", effect.Unk4.ToString(), delegate (string val) { effect.Unk4 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", effect.Unk5.ToString(), delegate (string val) { effect.Unk5 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", effect.Unk6.ToString(), delegate (string val) { effect.Unk6 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
