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

            form.CreateHeader("Condition Folder");

            form.CreateComboBox("Condition", (int)folder.Condition, Enum.GetNames(typeof(ConditionTable)), delegate(int index) { folder.Condition = (ConditionTable)index; });
        }
    }
}
