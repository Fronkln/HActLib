using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementLightPointWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementPointLight spot = node as DEElementPointLight;

            form.CreateHeader("Point Light");
            DEElementBaseSpotlightWindow.Draw(form, node);
        }
    }
}
