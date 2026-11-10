using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal class OENodeCharacterWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OENodeCharacter oeChar = node as OENodeCharacter;

            PropertyEditorBuilder.CreateSpace(15);

            PropertyEditorBuilder.CreateHeader("Character Info");
            PropertyEditorBuilder.CreateInput("Height", oeChar.Height.ToString(), delegate (string val) { oeChar.Height = uint.Parse(val); });
            PropertyEditorBuilder.CreateInput("Unknown 1", oeChar.Unk1.ToString(), delegate (string val) { oeChar.Unk1 = int.Parse(val); });
            PropertyEditorBuilder.CreateInput("Unknown 2", oeChar.Unk2.ToString(), delegate (string val) { oeChar.Unk2 = int.Parse(val); });
            PropertyEditorBuilder.CreateInput("Unknown 3", oeChar.Unk3.ToString(), delegate (string val) { oeChar.Unk3 = int.Parse(val); });
        }
    }
}
