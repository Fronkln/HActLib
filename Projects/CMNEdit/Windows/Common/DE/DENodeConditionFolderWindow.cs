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
                form.CreateHeader("Condition Folder");

                form.CreateComboBox("Condition", (int)folder.Condition, Enum.GetNames(typeof(ConditionTable)), delegate (int index) { folder.Condition = (ConditionTable)index; });
                form.CreateInput("Condition (ID)", ((int)folder.Condition).ToString(), delegate (string val) { folder.Condition = (ConditionTable)uint.Parse(val); }, NumberBox.NumberMode.UInt);
                form.CreateInput("Tag Value", folder.TagValue.ToString(), delegate (string val) { folder.TagValue = int.Parse(val); }, NumberBox.NumberMode.Int);
                form.CreateInput("PUID ID", folder.PuidID.ToString(), delegate (string val) { folder.PuidID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            }
            else
            {
                if(Form1.curGame == Game.YLAD)
                {
                    switch ((uint)folder.Condition)
                    {
                        case 133700001:
                            form.CreateHeader("Condition Folder (Custom)");
                            form.CreateInput("Job ID", folder.TagValue.ToString(), delegate (string val) { folder.TagValue = int.Parse(val); }, NumberBox.NumberMode.Int);
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
