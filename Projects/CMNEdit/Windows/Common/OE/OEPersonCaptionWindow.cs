using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEPersonCaptionWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEPersonCaption caption = node as OEPersonCaption;

            PropertyEditorBuilder.CreateHeader("Person Caption");

            PropertyEditorBuilder.CreateInput("Texture", caption.Texture.ToString(), delegate (string val) { caption.Texture = val; });

            PropertyEditorBuilder.CreateInput("Position X", caption.PosX.ToString(), delegate (string val) { caption.PosX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Position Y", caption.PosY.ToString(), delegate (string val) { caption.PosY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            PropertyEditorBuilder.CreateInput("Size X", caption.SizeX.ToString(), delegate (string val) { caption.SizeX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Size Y", caption.SizeY.ToString(), delegate (string val) { caption.SizeY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
