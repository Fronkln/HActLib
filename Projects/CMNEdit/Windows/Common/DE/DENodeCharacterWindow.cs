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
    internal class DENodeCharacterWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DENodeCharacter deChar = node as DENodeCharacter;

            form.CreateSpace(15);

            form.CreateHeader("Character Info");
            form.CreateInput("Character ID", deChar.CharacterID.ToString(), delegate (string var) { deChar.CharacterID = uint.Parse(var); }, NumberBox.NumberMode.Int);
            form.CreateComboBox("Scale ID", (int)deChar.ScaleID, Enum.GetNames(typeof(DECharacterScaleID)), delegate (int id) { deChar.ScaleID = (uint)id; });
            form.CreateInput("Option Flag", deChar.OptionFlag.ToString(), delegate (string var) { deChar.OptionFlag = uint.Parse(var); }, NumberBox.NumberMode.Int);
            form.CreateComboBox("Replace ID", (int)deChar.ReplaceID, Enum.GetNames(typeof(HActReplaceID)), delegate (int id) { deChar.ReplaceID = (HActReplaceID)id; });

            form.CreateSpace(15);

            if (Form1.curVer > GameVersion.Yakuza6Demo)
                for (int i = 0; i < deChar.ChangeCharacterID.Length; i++)
                        CreateReplaceIDField(deChar, form, i);
        }

        private static void CreateReplaceIDField(DENodeCharacter atk, Form1 form, int index)
        {
            form.CreateInput("Character Change ID " + (index + 1), atk.ChangeCharacterID[index].ToString(), delegate (string val) { atk.ChangeCharacterID[index] = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
