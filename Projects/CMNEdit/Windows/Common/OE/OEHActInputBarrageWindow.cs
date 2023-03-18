using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActInputBarrageWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActInputBarrage inf = node as OEHActInputBarrage;

            form.CreateSpace(25);
            form.CreateHeader("Input Barrage");

            form.CreateInput("Presses", inf.Presses.ToString(), delegate(string val) { inf.Presses = (uint)int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown 1", inf.Unk1.ToString(), delegate (string val) { inf.Unk1 = (uint)int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown 2", inf.Unk2.ToString(), delegate (string val) { inf.Unk2 = (uint)int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown 3", inf.Unk3.ToString(), delegate (string val) { inf.Unk3 = (uint)int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
