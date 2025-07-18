using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using static System.Windows.Forms.Design.AxImporter;

namespace CMNEdit
{
    internal static class CSVCharacterWindow
    {
        public static void Draw(Form1 form, CSVCharacter character, bool isCsvTree = false)
        {
            form.CreateHeader("Human (CSV)");

            if (isCsvTree)
                form.CreateInput("Name", character.Name, delegate (string val) { character.Name = val; Form1.Instance.SelectedNodeCsvTree.Text = val; });

            form.CreateComboBox("Linkout Type", character.ExitHActMode, Enum.GetNames<HumanReturnMode>(), delegate (int val) { character.ExitHActMode = val; }, isCsvTree: isCsvTree);
            form.CreateInput("Model Override", character.ModelOverride, delegate (string val) { character.ModelOverride = val; });

            form.CreateInput("Unknown", character.Unknown1.ToString(), delegate (string val) { character.Unknown1 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown2.ToString(), delegate (string val) { character.Unknown2 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown3.ToString(), delegate (string val) { character.Unknown3 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown4.ToString(), delegate (string val) { character.Unknown4 = int.Parse(val); });
            //form.CreateInput("Unknown", character.Unknown5.ToString(), delegate (string val) { character.Unknown5 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown6.ToString(), delegate (string val) { character.Unknown6 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown7.ToString(), delegate (string val) { character.Unknown7 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown8.ToString(), delegate (string val) { character.Unknown8 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown9.ToString(), delegate (string val) { character.Unknown9 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown10.ToString(), delegate (string val) { character.Unknown10 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown11.ToString(), delegate (string val) { character.Unknown11 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown12.ToString(), delegate (string val) { character.Unknown12 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown13.ToString(), delegate (string val) { character.Unknown13 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown14.ToString(), delegate (string val) { character.Unknown14 = int.Parse(val); });
            form.CreateInput("Unknown", character.Unknown15.ToString(), delegate (string val) { character.Unknown15 = int.Parse(val); });

            form.CreateButton("Edit Unknown Flag 5", delegate
            {
                CMNEdit.Windows.FlagEditor myNewForm = new CMNEdit.Windows.FlagEditor();
                myNewForm.Visible = true;
                myNewForm.Init(null, (uint)character.Unknown5, delegate (uint val) { character.Unknown5 = (int)val; Form1.EditingNode.Update(); });
            });
        }
    }
}
