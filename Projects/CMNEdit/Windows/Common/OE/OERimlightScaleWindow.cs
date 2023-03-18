using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OERimlightScaleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OERimlightScale rimScale = node as OERimlightScale;

            form.CreateHeader("Rim Light Scale");
            form.CreateInput("Scale", rimScale.Scale.ToString(), delegate (string val) { rimScale.Scale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
