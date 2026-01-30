using System;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;
using System.Reflection.Metadata;
using System.Globalization;

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
                form.CreateInput("Unknown Gaiden", inf.Data.Unknown.ToString(), delegate (string val) { inf.Data.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
           
            if(Form1.curGame >= Game.LADPYIH)
            {
                form.CreateInput("Unknown PYIH" , Convert.ToByte(inf.Data.Unknown2).ToString(), delegate (string val) { inf.Data.Unknown2 = byte.Parse(val) > 0; }, NumberBox.NumberMode.Byte);
                form.CreateInput("Unknown PYIH", Convert.ToByte(inf.Data.Unknown3).ToString(), delegate (string val) { inf.Data.Unknown3 = byte.Parse(val) > 0; }, NumberBox.NumberMode.Byte);
            }

            if (Form1.curGame >= Game.YK3)
            {
                form.CreateInput("Unknown YK3", inf.Data.Unknown4.ToString(CultureInfo.InvariantCulture), delegate (string val) { inf.Data.Unknown4 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                form.CreateInput("Unknown YK3", inf.Data.Unknown5.ToString(), delegate (string val) { inf.Data.Unknown5 = int.Parse(val); }, NumberBox.NumberMode.Int);
                form.CreateInput("Unknown YK3", inf.Data.Unknown6.ToString(), delegate (string val) { inf.Data.Unknown6 = int.Parse(val); }, NumberBox.NumberMode.Int); 
            }

            string[] options =  Enum.GetNames(DETimingInfoAttack.GetAttributesForGame(Form1.curGame));

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
