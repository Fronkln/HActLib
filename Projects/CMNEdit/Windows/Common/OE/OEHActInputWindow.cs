using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActInputWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActInput inf = node as OEHActInput;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("HAct Input");

            PropertyEditorBuilder.CreateInput("Duration",  new GameTick(inf.Timing).Frame.ToString(), delegate(string val) { inf.Timing = new GameTick(Utils.InvariantParse(val)).Tick; }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Unknown 1", inf.Unk1.ToString(), delegate (string val) { inf.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Unknown 2", inf.Unk2.ToString(), delegate (string val) { inf.Unk2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Unknown 3", inf.Unk3.ToString(), delegate (string val) { inf.Unk3 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
        }
    }
}
