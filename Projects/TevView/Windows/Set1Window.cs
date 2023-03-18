using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TevView
{
    internal static class Set1Window
    {
        public static void Draw(Form1 form, ObjectBase set)
        {   
            form.CreateHeader("Set 1");

            form.CreateInput("Unknown", set._InternalInfo.UnkNum1.ToString(), delegate(string val) { set._InternalInfo.UnkNum1 = uint.Parse(val); }, NumberBox.NumberMode.UInt)
;
            for(int i = 0; i < set.StringTable.Length; i++)
                CreateStringTable(form, set, i);
        }

        private static void CreateStringTable(Form1 form, ObjectBase set, int idx)
        {
            form.CreateInput("String", set.StringTable[idx], delegate (string val) { set.StringTable[idx] = val; });
        }
    }
}
