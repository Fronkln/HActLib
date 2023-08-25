using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementTimingInfoStunWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementTimingInfoStun stun = node as DEElementTimingInfoStun;

            form.CreateHeader("Transit Stun");

            form.CreateInput("Type", stun.Type.ToString(), delegate (string val) { stun.Type = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("GMT ID", stun.GmtID.ToString(), delegate (string val) { stun.GmtID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
