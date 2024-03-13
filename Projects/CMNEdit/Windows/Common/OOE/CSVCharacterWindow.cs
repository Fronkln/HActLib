using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class CSVCharacterWindow
    {
        public static void Draw(Form1 form, CSVCharacter character, bool isCsvTree = false)
        {
            if(isCsvTree)
                form.CreateInput("Name", character.Name, delegate (string val) { character.Name = val; }, isCsvTree: isCsvTree);

            form.CreateComboBox("Linkout Type", character.ExitHActMode, Enum.GetNames<HumanReturnMode>(), delegate (int val) { character.ExitHActMode = val; }, isCsvTree: isCsvTree);
            form.CreateInput("Model Override", character.ModelOverride, delegate (string val) { character.ModelOverride = val; }, isCsvTree: isCsvTree);
        }
    }
}
