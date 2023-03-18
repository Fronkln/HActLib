using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementStageWarpWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementStageWarp inf = node as DEElementStageWarp;

            form.CreateSpace(25);
            form.CreateHeader("Stage Warp");

            form.CreateInput("Warp ID", inf.WarpId.ToString(), delegate (string val) { inf.WarpId = (uint)int.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
