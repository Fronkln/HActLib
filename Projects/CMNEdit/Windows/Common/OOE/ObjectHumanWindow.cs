using HActLib;
using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class ObjectHumanWindow
    {
        public static void Draw(Form1 form, ObjectHuman obj)
        {
            CSVCharacter csvEntry = Form1.TevCsvEntry.TryGetHActCharacter(obj.Replace);

            PropertyEditorBuilder.CreateHeader("Human");

            PropertyEditorBuilder.CreateInput("Name", obj.Replace, delegate (string val) { obj.Replace = val; });
            PropertyEditorBuilder.CreateInput("Object Type", obj.ObjectType, delegate (string val) { obj.ObjectType = val; });
            PropertyEditorBuilder.CreateInput("Body", obj.BodyType, delegate (string val) { obj.BodyType = val; });

            PropertyEditorBuilder.CreateInput("Height", obj.Height.ToString(), delegate (string val) { obj.Height = int.Parse(val); }, NumberBox.NumberMode.Int);

            if (csvEntry != null)
            {
                CSVCharacterWindow.Draw(form, csvEntry, false);
            }

        }
    }
}
