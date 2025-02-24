using System;
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

            form.CreateInput("Damage", inf.Data.Damage.ToString(), delegate (string val) { inf.Data.Damage = uint.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Power", inf.Data.Power.ToString(), delegate (string val) { inf.Data.Power = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Flag", inf.Data.Flag.ToString(), delegate (string val) { inf.Data.Flag = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

            form.CreateInput("Part", inf.Data.Parts.ToString(), delegate (string val) { inf.Data.Parts = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            if (Form1.curVer >= GameVersion.DE2)
                form.CreateInput("Attack ID", inf.Data.AttackID.ToString(), delegate (string val) { inf.Data.AttackID = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Attributes", inf.Data.Attributes.ToString(), delegate (string val) { inf.Data.Attributes = ulong.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.Long);

            if (Form1.curGame == Game.LAD7Gaiden || Form1.curGame >= Game.LADPYIH)
                form.CreateInput("Unknown", inf.Data.Unknown.ToString(), delegate (string val) { inf.Data.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
           
            if(Form1.curGame >= Game.LADPYIH)
                form.CreateInput("Unknown", inf.Data.Unknown2.ToString(), delegate (string val) { inf.Data.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);

            string[] options = null;

            switch(Form1.curGame)
            {
                default:
                    options = Enum.GetNames<BattleAttributeLJ>();
                    break;
                case Game.Y6Demo:
                    options = Enum.GetNames<BattleAttributeYK2>();
                    break;
                case Game.Y6:
                    options = Enum.GetNames<BattleAttributeYK2>();
                    break;
                case Game.YK2:
                    options = Enum.GetNames<BattleAttributeYK2>();
                    break;
                case Game.JE:
                    options = Enum.GetNames<BattleAttributeJE>();
                    break;
                case Game.YLAD:
                    options = Enum.GetNames<BattleAttributeYLAD>();
                    break;
                case Game.LJ:
                    options = Enum.GetNames<BattleAttributeLJ>();
                    break;
                case Game.LAD7Gaiden:
                    options = Enum.GetNames<BattleAttributeGaiden>();
                    break;
                case Game.LADIW:
                    options = Enum.GetNames<BattleAttributeIW>();
                    break;
            }

            form.CreateButton("Edit Attributes", delegate
            {
                CMNEdit.Windows.FlagEditor64 myNewForm = new CMNEdit.Windows.FlagEditor64();
                myNewForm.Visible = true;
                myNewForm.Init(options, inf.Data.Attributes, delegate(ulong val) { inf.Data.Attributes = val; Form1.EditingNode.Update(); });
            });

            form.CreateButton("Edit Parts", delegate
            {
                CMNEdit.Windows.FlagEditor64 myNewForm = new CMNEdit.Windows.FlagEditor64();
                myNewForm.Visible = true;
                myNewForm.Init(Enum.GetNames<BattleColliball>(), inf.Data.Parts, delegate(ulong val) { inf.Data.Parts = (uint)val; });
            });
        }
    }
}
