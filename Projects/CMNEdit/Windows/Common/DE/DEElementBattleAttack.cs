using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;
using System.Reflection.Metadata;

namespace CMNEdit.Windows.Common.DE
{
    public static class DEElementBattleAttackWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DETimingInfoAttack inf = node as DETimingInfoAttack;

            form.CreateSpace(25);
            form.CreateHeader("Battle Attack");

            form.CreateInput("Damage", inf.Data.Damage.ToString(), delegate (string val) { inf.Data.Damage = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Power", inf.Data.Power.ToString(), delegate (string val) { inf.Data.Power = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Flag", inf.Data.Flag.ToString(), delegate (string val) { inf.Data.Flag = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

            form.CreateInput("Part", inf.Data.Parts.ToString(), delegate (string val) { inf.Data.Parts = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            if (Form1.curVer == GameVersion.DE2)
                form.CreateInput("Attack ID", inf.Data.AttackID.ToString(), delegate (string val) { inf.Data.AttackID = int.Parse(val); }, NumberBox.NumberMode.Int);

            for (int i = 0; i < inf.Data.Attributes.Length; i++)
                CreateAttributeField(inf, form, i);
        }

        private static void CreateAttributeField(DETimingInfoAttack atk, Form1 form, int index)
        {
            form.CreateInput("Attribute " + (index + 1), atk.Data.Attributes[index].ToString(), delegate (string val) { atk.Data.Attributes[index] = byte.Parse(val); }, NumberBox.NumberMode.Byte);
        }
    }
}
