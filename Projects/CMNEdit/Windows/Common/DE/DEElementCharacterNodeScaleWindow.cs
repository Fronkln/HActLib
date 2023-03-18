using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementCharacterNodeScaleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCharacterNodeScale nodeScale = node as DEElementCharacterNodeScale;

            form.CreateHeader("Character Scale");
            form.CreateInput("Head Scale", nodeScale.HeadScale.ToString(), delegate (string val) { nodeScale.HeadScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Hand Scale", nodeScale.HandScale.ToString(), delegate (string val) { nodeScale.HandScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
