using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit.Windows
{
    internal static class DEEelementFollowupWindowWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementFollowupWindow wind = node as DEElementFollowupWindow;

            form.CreateHeader("Followup Window");
            form.CreateInput("Param", wind.Unknown.ToString(), delegate (string val) { wind.Unknown = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
