using System;
using HActLib;

namespace CMNEdit
{
    internal static class OEHActBranchWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEHActBranch branch = node as OEHActBranch;

            form.CreateHeader("Branching");

            form.CreateInput("Unknown", branch.Unk1.ToString(), delegate (string val) { branch.Unk1 = short.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Unknown", branch.Unk2.ToString(), delegate (string val) { branch.Unk2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", branch.Unk3.ToString(), delegate (string val) { branch.Unk3 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", branch.Unk4.ToString(), delegate (string val) { branch.Unk4 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", branch.Unk5.ToString(), delegate (string val) { branch.Unk5 = int.Parse(val); }, NumberBox.NumberMode.Int);
        }
    }
}
