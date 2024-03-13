using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class Set2Element1019Window
    {
        public static void Draw(Form1 form, Set2Element1019 element)
        {
            form.CreateHeader("HAct Event (HE)");
            form.CreateInput("Type", element.Type1019, delegate(string val) { element.Type1019 = val; });
        }
    }
}
