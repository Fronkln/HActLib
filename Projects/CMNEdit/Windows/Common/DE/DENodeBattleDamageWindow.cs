using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;


namespace CMNEdit.Windows.Common.DE
{
    internal static class DENodeBattleDamageWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeBattleDamage inf = node as NodeBattleDamage;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Damage");

            PropertyEditorBuilder.CreateInput("Damage", inf.Damage.ToString(), delegate (string val) { inf.Damage = uint.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Force Dead", inf.ForceDead.ToString(), delegate (string val) { inf.ForceDead = int.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("No Dead", Convert.ToInt32(inf.NoDead).ToString(), delegate (string val) { inf.NoDead = byte.Parse(val) > 0; Form1.EditingNode.Update(); }, NumberBox.NumberMode.Byte);
            PropertyEditorBuilder.CreateInput("Vanish", inf.Vanish.ToString(), delegate (string val) { inf.Vanish = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Fatal", inf.Fatal.ToString(), delegate (string val) { inf.Fatal = int.Parse(val); }, NumberBox.NumberMode.Int);

            PropertyEditorBuilder.CreateInput("Recover", inf.Recover.ToString(), delegate (string val) { inf.Recover = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Target Sync", inf.TargetSync.ToString(), delegate (string val) { inf.TargetSync = int.Parse(val); }, NumberBox.NumberMode.Int);
            PropertyEditorBuilder.CreateInput("Attacker", inf.Attacker.ToString(), delegate (string val) { inf.Attacker = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            if (Form1.curVer >= GameVersion.DE2)
            {
                PropertyEditorBuilder.CreateInput("Direct Damage", inf.DirectDamage.ToString(), delegate (string val) { inf.DirectDamage = int.Parse(val); }, NumberBox.NumberMode.Int);
                PropertyEditorBuilder.CreateInput("Direct Damage Is HP Ratio", inf.DirectDamageIsHpRatio.ToString(), delegate (string val) { inf.DirectDamageIsHpRatio = int.Parse(val); }, NumberBox.NumberMode.Int);
                PropertyEditorBuilder.CreateInput("Attack ID", inf.AttackId.ToString(), delegate (string val) { inf.AttackId = int.Parse(val); }, NumberBox.NumberMode.Int);
            }
        }
    }
}
