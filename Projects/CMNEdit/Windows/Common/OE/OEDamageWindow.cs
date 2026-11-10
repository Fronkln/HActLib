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

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Damage");

            PropertyEditorBuilder.CreateInput("Damage",  inf.Damage.ToString(), delegate(string val) { inf.Damage = int.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Unknown", inf.Unk1.ToString(), delegate (string val) { inf.Unk1 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Unknown", inf.Unk2.ToString(), delegate (string val) { inf.Unk2 = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Force Dead", inf.ForceDead.ToString(), delegate (string val) { inf.ForceDead = int.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Vanish", inf.Vanish.ToString(), delegate (string val) { inf.Vanish = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Dont Kill", Convert.ToByte(inf.NoDead).ToString(), delegate (string val) { inf.NoDead = byte.Parse(val) == 1; Form1.EditingNode.Update(); }, NumberBox.NumberMode.Byte);
        }
    }
}
