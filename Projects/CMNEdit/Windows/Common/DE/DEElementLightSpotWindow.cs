using HActLib;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParLibrary;
using System.Drawing;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class DEElementLightSpotWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementSpotLight spot = node as DEElementSpotLight;

            form.CreateHeader("Spot Light");
            DEElementBaseSpotlightWindow.Draw(form, node);
        }
    }
}
