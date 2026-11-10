using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DENodeConditionFolderWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DENodeConditionFolder folder = node as DENodeConditionFolder;

            if (!IsCustomCondition(folder))
            {
                PropertyEditorBuilder.CreateHeader("Condition Folder");

                PropertyEditorBuilder.CreateComboBox("Condition", (int)folder.Condition, Enum.GetNames(typeof(ConditionTable)), delegate (int index) { folder.Condition = (ConditionTable)index; });
                PropertyEditorBuilder.CreateInput("Condition (ID)", ((int)folder.Condition).ToString(), delegate (string val) { folder.Condition = (ConditionTable)uint.Parse(val); }, NumberBox.NumberMode.UInt);
                PropertyEditorBuilder.CreateInput("Tag Value", folder.TagValue.ToString(), delegate (string val) { folder.TagValue = int.Parse(val); }, NumberBox.NumberMode.Int);
                PropertyEditorBuilder.CreateInput("PUID ID", folder.PuidID.ToString(), delegate (string val) { folder.PuidID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
                PropertyEditorBuilder.CreateInput("Logical Operator?", folder.DataFlag.ToString(), delegate (string val) { folder.DataFlag = uint.Parse(val); }, NumberBox.NumberMode.UInt);
                PropertyEditorBuilder.CreateInput("Unknown", folder.Unknown1.ToString(), delegate (string val) { folder.Unknown1 = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            }
            else
            {
                if(Form1.curGame == Game.YLAD)
                {
                    switch ((uint)folder.Condition)
                    {
                        case 133700001:
                            PropertyEditorBuilder.CreateHeader("Condition Folder (Custom)");
                            PropertyEditorBuilder.CreateInput("Job ID", folder.TagValue.ToString(), delegate (string val) { folder.TagValue = int.Parse(val); }, NumberBox.NumberMode.Int);
                            break;
                    }
                }
            }
        }



        private static bool IsCustomCondition(DENodeConditionFolder cond)
        {
            if(Form1.curGame == Game.YLAD)
            {
                switch ((uint)cond.Condition)
                {
                    case 133700001:
                        return true;
                }
            }

            return false;
        }
    }
}
